using System;
using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.Config;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotification.Helpers
{
    public abstract class UploadDiagnosisKeysHelper
    {
		/// <summary>
		/// This method does a simple validation of keys that might be obtained from the operating system.
		/// Some actions by user, may cause wrond TEKs being generated and obtained from OS, e.g., manually changing the date.
		/// Xamarin representation of TEKs does not take care of such things so we do it in this method.
		/// The method will return a list with only valid keys.
		///
		/// These are the factors that make a key upload invalid according to Apple at
		/// https://developer.apple.com/documentation/exposurenotification/setting_up_an_exposure_notification_server:
		///  1. RollingStart is not unique for each of the user's keys
		///  2. There are gaps in the key validity periods for a user 
		///		(they say "gaps in ENIntervalNumber", but I think this is a more correct way of saying what they mean)
		///  3. There are keys with overlapping validity periods
		///  4. The period of time covered exceeds 14 days
		///  5. RollingPeriod is not 1 day
		/// </summary>
		/// <param name="temporaryExposureKeys"></param>
		/// <returns></returns>
		public static List<ExposureKeyModel> CreateAValidListOfTemporaryExposureKeys(IEnumerable<ExposureKeyModel> temporaryExposureKeys)
		{
			List<ExposureKeyModel> validListOfTeks = temporaryExposureKeys.ToList() ;

			// Satisfy criterion 1. If there are several keys with the same RollingStart, keep only 1

			int i = 0;
			while (i < validListOfTeks.Count)
            {
                ExposureKeyModel leftTek = validListOfTeks[i];

				// Remove all duplicates to the right of the key at i

				int j = validListOfTeks.Count - 1;
				while (j > i)
                {
                    ExposureKeyModel rightTek = validListOfTeks[j];

					if (leftTek.RollingStart == rightTek.RollingStart)
					{
						validListOfTeks.RemoveAt(j);
					}
					j--;
				}

				i++;
			}

			// Satisfy criterion 2. Iterate backwards in time and if the next key is not 1 day before the previous key
            // take what we have iterated over so far

			// Sort from newest to oldest
			validListOfTeks.Sort((x, y) => y.RollingStart.CompareTo(x.RollingStart));
			for (int k = 0; k < validListOfTeks.Count - 1; k++)
			{
				if (validListOfTeks[k + 1].RollingStart != validListOfTeks[k].RollingStart.AddDays(-1))
				{
					validListOfTeks = validListOfTeks.Take(k + 1).ToList();
					break;
				}
			}

			// Criterion 3 will be satisfied. We now have unique RollingStarts and each key is 1 day from the next. Criterion 3 will
			// then be satisfied when we satisfy criterion 5 below

			// Satisfy criterion 4. If we have more than 14 keys, only take 14
			if (validListOfTeks.Count > 14)
            {
				validListOfTeks = validListOfTeks.Take(14).ToList();
            }

			// Satisfy criterion 5 by setting RollingDuration to 1 day
			foreach (ExposureKeyModel tek in validListOfTeks)
            {
				tek.RollingDuration = new TimeSpan(1, 0, 0, 0);

			}

			return validListOfTeks;
		}

		/// <summary>
		/// This method is used to set TransmissionRiskLevel before keys are sent to server.
		/// It operates on intervals that are set in configuration file, these intervals are defined strictly, without any overlaps.
		/// The method does convertion from UTC time representation of symptomOnset date to DateTimeOffset, then gets
		/// a positive or negative integer representing difference between key's DateTimeOffset and the symptoms DateTimeOffset.
		/// Based on configuration, TransmissionRiskLevel for the key is then set to the natural index in the table TransmissionRiskLevel table.
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
		public static List<ExposureKeyModel> SetTransmissionRiskLevel(List<ExposureKeyModel> keys, DateTime symptomsDate)
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
