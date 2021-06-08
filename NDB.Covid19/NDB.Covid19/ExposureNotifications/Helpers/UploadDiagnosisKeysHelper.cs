using System;
using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers
{
    public abstract class UploadDiagnosisKeysHelper
    {
		/// <summary>
		/// This method does a simple validation of keys that might be obtained from the operating system.
		/// Some actions by user, may cause wrond TEKs being generated and obtained from OS, e.g., manually changing the date.
		/// Xamarin representation of TEKs does not take care of such things so we do it in this method.
		/// The method will return a list with only valid keys.
		///
		/// These are the factors [per April 2021] that make a key upload invalid according to Apple at
		/// https://developer.apple.com/documentation/exposurenotification/setting_up_an_exposure_notification_server:
		/// 1. The period of time covered by the data file exceeds 14 days
		/// 2. TEKRollingPeriod, if included, must be a positive value that is no greater than 1440 minutes
		/// </summary>
		/// <param name="temporaryExposureKeys"></param>
		/// <returns></returns>
		public static List<ExposureKeyModel> CreateAValidListOfTemporaryExposureKeys(IEnumerable<ExposureKeyModel> temporaryExposureKeys)
		{
			List<ExposureKeyModel> validListOfTeks =
				temporaryExposureKeys
				.Where(
					tek => (tek.RollingStart.Date > SystemTime.Now().Date.AddDays(-14)
					&& tek.RollingStart.Date <= SystemTime.Now().Date))
				.ToList();

			foreach (ExposureKeyModel tek in validListOfTeks)
			{
				if (tek.RollingDuration.TotalMinutes < 10
					|| tek.RollingDuration.TotalMinutes > 1440)
				{
					tek.RollingDuration = new TimeSpan(1, 0, 0, 0);
				}
			}

			if (validListOfTeks.Count() != temporaryExposureKeys.Count())
			{
				LogUtils.LogMessage(Enums.LogSeverity.WARNING, "Number of keys filtered out by Smittestopp: " + (temporaryExposureKeys.Count() - validListOfTeks.Count()));
			}

			return validListOfTeks;
		}

		/// <summary>
		/// This method is used to set DSOS and TransmissionRiskLevel before keys are sent to server.
		/// It operates on intervals that are set in configuration file, these intervals are defined strictly, without any overlaps.
		/// The method does convertion from UTC time representation of symptomOnset date to DateTimeOffset, then gets
		/// a positive or negative integer representing difference between key's DateTimeOffset and the symptoms DateTimeOffset.
		/// Based on configuration, TransmissionRiskLevel for the key is then set to the natural index in the table TransmissionRiskLevel table.
		/// NOTE: TransmissionRisk is used by clients who still use EN API v1. In EN API v2 DSOS is used instead.
		/// NOTE: By natural we mean that the first index is 1, not 0, the second is 2, not 1 etc.
		/// NOTE: The key risk will always be assigned to 1 (lowest risk value) at the end, if none other higher value is assigned. 
		/// Example: Key date: 26.05.2020, Symptoms onset: 24.05.2020.
		///  The difference in dates is +2 days.
		///  In Configuration file in the tuples array it corresponds to the item where this difference is set
		///  has index 3. See Tuple.Create(-1, 2) in Conf.cs This means, that the natural number index equals 4.
		///  The key's TransmissionRiskLevel would be set to 4.
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="symptomsDate"></param>
		/// <returns></returns>
		public static List<ExposureKeyModel> SetTransmissionRiskAndDSOS(List<ExposureKeyModel> keys, DateTime symptomsDate)
		{
			DateTimeOffset dateTimeOffsetForSymptomDate = new DateTimeOffset(symptomsDate);
			foreach (ExposureKeyModel key in keys)
			{
				TimeSpan timeSpanBetweenSymptomOnsetAndKey = key.RollingStart - dateTimeOffsetForSymptomDate;
				int daysBetweenSymptomOnsetAndKey = timeSpanBetweenSymptomOnsetAndKey.Days;
                key.DaysSinceOnsetOfSymptoms = daysBetweenSymptomOnsetAndKey;
				for (int i = Conf.DAYS_SINCE_ONSET_FOR_TRANSMISSION_RISK_CALCULATION.Length - 1; i >= 0; i--)
				{
					if (daysBetweenSymptomOnsetAndKey >= Conf.DAYS_SINCE_ONSET_FOR_TRANSMISSION_RISK_CALCULATION[i].Item1
						&& daysBetweenSymptomOnsetAndKey <= Conf.DAYS_SINCE_ONSET_FOR_TRANSMISSION_RISK_CALCULATION[i].Item2)
					{
						key.TransmissionRiskLevel = (RiskLevel)i + 1; // Setting to natural index number since 0 represents Invalid
						break;
					}
				}
			}
			return keys;
		}
	}
}
