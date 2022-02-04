using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using static NDB.Covid19.ProtoModels.TemporaryExposureKey.Types.ReportType;
using static NDB.Covid19.OAuth2.AuthenticationState;

namespace NDB.Covid19.Models
{
    public class SelfDiagnosisSubmissionDTO
    {
        public IEnumerable<ExposureKeyModel> Keys { get; set; }
        public List<string> Regions { get; set; }
        public List<string> VisitedCountries { get; set; } = new List<string>();
        public string AppPackageName { get; set; }
        public string Platform { get; set; }
        public string Padding { get; set; }
        public bool IsSharingAllowed { get; set; } = false;
        public int ReportType { get; set; }

        // For unit testing
        // Moq library requires public constructor that is overwritten by parametrized constructor
        public SelfDiagnosisSubmissionDTO()
        {
            AppPackageName = ServiceLocator.Current.GetInstance<IAppInfo>().PackageName;
            Platform = ServiceLocator.Current.GetInstance<IDeviceInfo>().Platform.ToString();
            Regions = Conf.SUPPORTED_REGIONS.ToList();
            IsSharingAllowed = LocalPreferencesHelper.AreCountryConsentsGiven;
            ReportType = LocalPreferencesHelper.IsReportingSelfTest ? (int)SelfReport : (int)ConfirmedTest;

            // Denmark will be added automatically on server side. Only additional countries should be added here.
            VisitedCountries.AddRange(PersonalData?.VisitedCountries ?? new List<string>());
        }

        public SelfDiagnosisSubmissionDTO(IEnumerable<ExposureKeyModel> keys) : this()
        {
            Keys = keys;
            ComputePadding();
        }

        // Computes hash of random length so packets with keys in network will have different size
        // Usualy generated hash is ~60 bytes UTF8, while server recommends padding of random size from 1 to 2 KB.
        // We append random hashes random amount of time so the padding will be within recommended intervals.
        public void ComputePadding()
        {
            Padding = "";
            Random random = new Random();
            int minRepeatTimes = random.Next(12, 24);

            for (int i = 1; i <= minRepeatTimes; ++i)
            {
                byte[] bytes = 
                    SHA256.Create()
                        .ComputeHash(
                            Encoding.UTF8.GetBytes(
                                AppPackageName +
                                Platform +
                                DateTime.UtcNow.Ticks));
                string base64String = Convert.ToBase64String(bytes);
                if (Padding.Length * 2 < 1024 ||
                    (base64String + Padding).Length * 2 <= 2048)
                {
                    Padding += base64String;
                }
                else
                {
                    break;
                }
            }
        }
    }
}