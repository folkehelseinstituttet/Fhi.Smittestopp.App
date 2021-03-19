﻿using System;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.WebServices.ExposureNotification;

namespace NDB.Covid19.PersistedData
{
    public static class LocalPreferencesHelper
    {
        static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        public static int MigrationCount
        {
            get => _preferences.Get(PreferencesKeys.MIGRATION_COUNT, 0);
            set => _preferences.Set(PreferencesKeys.MIGRATION_COUNT, value);
        }

        //Used to determine if onboarding should be shown, or the user should be sent directly to the result page.
        public static bool IsOnboardingCompleted
        {
            get => _preferences.Get(PreferencesKeys.IS_ONBOARDING_COMPLETED_PREF, false);
            set => _preferences.Set(PreferencesKeys.IS_ONBOARDING_COMPLETED_PREF, value);
        }

        // USed to distinguish if user already has seen WhatIsNew page
        public static bool IsOnboardingCountriesCompleted
        {
            get => _preferences.Get(PreferencesKeys.IS_ONBOARDING_COUNTRIES_COMPLETED_PREF, false);
            set => _preferences.Set(PreferencesKeys.IS_ONBOARDING_COUNTRIES_COMPLETED_PREF, value);
        }

        public static bool GetIsDownloadWithMobileDataEnabled() => _preferences.Get(PreferencesKeys.USE_MOBILE_DATA_PREF, true);
        public static void SetIsDownloadWithMobileDataEnabled(bool isDownloadWithMobileDataEnabled)
        {
            _preferences.Set(PreferencesKeys.USE_MOBILE_DATA_PREF, isDownloadWithMobileDataEnabled);
        }

        // This is the date of the last fetch, which is displayed to the user on the Messages screen.
        //  The keys are not submitted yet
        public static DateTime GetUpdatedDateTime() => _preferences.Get(PreferencesKeys.MESSAGES_LAST_UPDATED_PREF, DateTime.MinValue);
        public static void UpdateLastUpdatedDate()
        {
            _preferences.Set(PreferencesKeys.MESSAGES_LAST_UPDATED_PREF, SystemTime.Now());
        }

        // The date of the last successful pulling of the keys
        // Keys that were successfully pulled AND submitted to the EN API.
        public static DateTime GetLastPullKeysSucceededDateTime() => _preferences.Get(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, DateTime.MinValue);
        public static void UpdateLastPullKeysSucceededDateTime()
        {
            _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now());

            int batchNumber = LastPullKeysBatchNumberNotSubmitted;
            LastPullKeysBatchNumberSuccessfullySubmitted = batchNumber;
        }

        //The last batch that was successfully fetched but not yet submitted to the EN API.
        public static int LastPullKeysBatchNumberNotSubmitted
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED, 0);
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED, value);
        }

        //The last batch that was successfully fetched AND submitted to the EN API.
        public static int LastPullKeysBatchNumberSuccessfullySubmitted
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED, 0);
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED, value);
        }

        //The last pulled batch type, which is needed to know when to reset the batch number to 1 after approving the terms. See PullKeysParams.cs
        public static BatchType LastPulledBatchType
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_TYPE, "no").ToBatchType();
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_TYPE, value.ToTypeString());
        }

        public static string GetAppLanguage() => _preferences.Get(PreferencesKeys.APP_LANGUAGE, null);
        public static void SetAppLanguage(string language)
        {
            _preferences.Set(PreferencesKeys.APP_LANGUAGE, language);
        }

        // The date of when the last update to numbers pulled from FHI were, which is used on the "Daily numbers" page.
        public static DateTime FHILastUpdateDateTime
        {
            get => _preferences.Get(PreferencesKeys.FHI_DATA_LAST_UPDATED_PREF, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.FHI_DATA_LAST_UPDATED_PREF, value);
        }

        // The date of when the last update to the download numbers were, which is used on the "Daily numbers" page.
        public static DateTime APPDownloadNumberLastUpdateDateTime
        {
            get => _preferences.Get(PreferencesKeys.APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF, value);
        }

        public static DateTime LastDateTimeTermsNotificationWasShown
        {
            get => _preferences.Get(PreferencesKeys.LAST_TERMS_NOTIFICATION_DATE_TIME, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.LAST_TERMS_NOTIFICATION_DATE_TIME, value);
        }

        public static double ExposureTimeThreshold
        {
            get => _preferences.Get(PreferencesKeys.EXPOSURE_TIME_THRESHOLD, Conf.EXPOSURE_TIME_THRESHOLD);
            set => _preferences.Set(PreferencesKeys.EXPOSURE_TIME_THRESHOLD, value);
        }

        public static double LowAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.LOW_ATTENUATION_DURATION_MULTIPLIER, Conf.LOW_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.LOW_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static double MiddleAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.MIDDLE_ATTENUATION_DURATION_MULTIPLIER, Conf.MIDDLE_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.MIDDLE_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static double HighAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.HIGH_ATTENUATION_DURATION_MULTIPLIER, Conf.HIGH_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.HIGH_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static DateTime LastPermissionsNotificationDateTimeUtc
        {
            get => _preferences.Get(PreferencesKeys.LAST_PERMISSIONS_NOTIFICATION_DATE_TIME, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.LAST_PERMISSIONS_NOTIFICATION_DATE_TIME, value);
        }

        public static DateTime LastNTPUtcDateTime
        {
            get => _preferences.Get(PreferencesKeys.LAST_NTP_UTC_DATE_TIME, Conf.DATE_TIME_REPLACEMENT);
            set => _preferences.Set(PreferencesKeys.LAST_NTP_UTC_DATE_TIME, value);
        }

        public static bool AreCountryConsentsGiven
        {
            get => _preferences.Get(PreferencesKeys.COUNTRY_CONSENTS_GIVEN, false);
            set => _preferences.Set(PreferencesKeys.COUNTRY_CONSENTS_GIVEN, value);
        }

         public static bool DidFirstFileOfTheDayEndedWith204
        {
            get => _preferences.Get(PreferencesKeys.FETCHING_ACROSS_DATES_204_FIRST_BATCH, false);
            set => _preferences.Set(PreferencesKeys.FETCHING_ACROSS_DATES_204_FIRST_BATCH, value);
        }

        public static bool HasNeverSuccessfullyFetchedFHIData
        {
            get => _preferences.Get(PreferencesKeys.FHI_DATA_HAS_NEVER_BEEN_CALLED, true);
            set => _preferences.Set(PreferencesKeys.FHI_DATA_HAS_NEVER_BEEN_CALLED, value);
        }

        //Data from FHI
        public static class DailyNumbers
        {
            public static void UpdateAll(DailyNumbersDTO dto)
            {
                FHILastUpdateDateTime = dto.CovidStatistics.Date;
                FHIConfirmedCasesToday = dto.CovidStatistics.ConfirmedCasesToday;
                FHIConfirmedCasesTotal = dto.CovidStatistics.ConfirmedCasesTotal;
                FHITestsConductedToday = dto.CovidStatistics.TestsConductedToday;
                FHITestsConductedTotal = dto.CovidStatistics.TestsConductedTotal;
                FHIPatientsAdmittedToday = dto.CovidStatistics.PatientsAdmittedToday;
                FHIPatientsIntensiveCare = dto.CovidStatistics.IcuAdmittedToday;
                FHIVaccinationsDoseOneToday = dto.CovidStatistics.VaccinatedFirstDoseToday;
                FHIVaccinationsDoseTwoToday = dto.CovidStatistics.VaccinatedSecondDoseToday;
                FHIVaccinationsDoseOneTotal = dto.CovidStatistics.VaccinatedFirstDoseTotal;
                FHIVaccinationsDoseTwoTotal = dto.CovidStatistics.VaccinatedSecondDoseTotal;
                APPNumberOfPositiveTestsResultsLast7Days = dto.ApplicationStatistics.PositiveResultsLast7Days;
                APPNumberOfPositiveTestsResultsTotal = dto.ApplicationStatistics.PositiveTestsResultsTotal;
                APPSmittestopDownloadsTotal = dto.ApplicationStatistics.SmittestopDownloadsTotal;
                APPDownloadNumberLastUpdateDateTime = dto.ApplicationStatistics.EntryDate;
                HasNeverSuccessfullyFetchedFHIData = false;
            }

            public static int FHIConfirmedCasesToday
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_CONFIRMED_CASES_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_CONFIRMED_CASES_TODAY_PREF, value);
            }

            public static int FHIConfirmedCasesTotal
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_CONFIRMED_CASES_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_CONFIRMED_CASES_TOTAL_PREF, value);
            }

            public static int FHITestsConductedToday
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_TESTS_CONDUCTED_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_TESTS_CONDUCTED_TODAY_PREF, value);
            }

            public static int FHITestsConductedTotal
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_TESTS_CONDUCTED_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_TESTS_CONDUCTED_TOTAL_PREF, value);
            }

            public static int FHIPatientsAdmittedToday
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_PATIENTS_ADMITTED_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_PATIENTS_ADMITTED_TODAY_PREF, value);
            }

            public static int FHIPatientsIntensiveCare
            {
                get => _preferences.Get(PreferencesKeys.FHI_DATA_PATIENTS_INTENSIVE_CARE_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_DATA_PATIENTS_INTENSIVE_CARE_PREF, value);
            }

            public static int FHIVaccinationsDoseOneToday
            {
                get => _preferences.Get(PreferencesKeys.FHI_VACCINATIONS_DOSE_ONE_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_VACCINATIONS_DOSE_ONE_TODAY_PREF, value);
            }

            public static int FHIVaccinationsDoseTwoToday
            {
                get => _preferences.Get(PreferencesKeys.FHI_VACCINATIONS_DOSE_TWO_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_VACCINATIONS_DOSE_TWO_TODAY_PREF, value);
            }

            public static int FHIVaccinationsDoseOneTotal
            {
                get => _preferences.Get(PreferencesKeys.FHI_VACCINATIONS_DOSE_ONE_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_VACCINATIONS_DOSE_ONE_TOTAL_PREF, value);
            }

            public static int FHIVaccinationsDoseTwoTotal
            {
                get => _preferences.Get(PreferencesKeys.FHI_VACCINATIONS_DOSE_TWO_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.FHI_VACCINATIONS_DOSE_TWO_TOTAL_PREF, value);
            }

            public static int APPNumberOfPositiveTestsResultsLast7Days
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF, value);
            }

            public static int APPNumberOfPositiveTestsResultsTotal
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF, value);
            }

            public static int APPSmittestopDownloadsTotal
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF, value);
            }
        }
    }
}
