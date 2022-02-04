using System;
using System.Globalization;
using I18NPortable;
#if APPCENTER
using Microsoft.AppCenter.Crashes;
#endif
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using NDB.Covid19.ProtoModels;
using Xamarin.Auth;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class InformationAndConsentViewModel
    {
        public static string INFORMATION_CONSENT_HEADER_TEXT => "INFOCONSENT_HEADER".Translate();
        public static string INFOCONSENT_DESCRIPTION => "INFOCONSENT_DESCRIPTION".Translate();
        public static string INFORMATION_CONSENT_ID_PORTEN_BUTTON_TEXT => "INFOCONSENT_LOGIN".Translate();

        public static string CLOSE_BUTTON_ACCESSIBILITY_LABEL => "SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON".Translate();

        public static string INFOCONSENT_LOOKUP_HEADER => "INFOCONSENT_LOOKUP_HEADER".Translate();
        public static string INFOCONSENT_LOOKUP_TEXT => "INFOCONSENT_LOOKUP_TEXT".Translate();
        public static string INFOCONSENT_NOTIFICATION_HEADER => "INFOCONSENT_NOTIFICATION_HEADER".Translate();
        public static string INFOCONSENT_NOTIFICATION_TEXT => "INFOCONSENT_NOTIFICATION_TEXT".Translate();
        public static string INFOCONSENT_CONSENT_BEAWARE_TEXT => "INFOCONSENT_CONSENT_BEAWARE_TEXT".Translate();
        public static string INFOCONSENT_CONSENT_EXPLANATION_TEXT => "INFOCONSENT_CONSENT_EXPLANATION_TEXT".Translate();
        public static string INFOCONSENT_CONSENTSWITCH_LABEL => "INFOCONSENT_CONSENTSWITCH_LABEL".Translate();

        public static string INFOCONSENT_SELF_TEST_HEADER => "INFOCONSENT_SELF_TEST_HEADER".Translate();
        public static string INFOCONSENT_SELF_TEST_DESCRIPTION => "INFOCONSENT_SELF_TEST_DESCRIPTION".Translate();
        public static string INFOCONSENT_SELF_TEST_LOOKUP_HEADER => "INFOCONSENT_SELF_TEST_LOOKUP_HEADER".Translate();
        public static string INFOCONSENT_SELF_TEST_LOOKUP_TEXT => "INFOCONSENT_SELF_TEST_LOOKUP_TEXT".Translate();
        public static string INFOCONSENT_SELF_TEST_NOTIFICATION_TEXT => "INFOCONSENT_SELF_TEST_NOTIFICATION_TEXT".Translate();
        public static string INFOCONSENT_SELF_TEST_CONSENT_EXPLANATION_TEXT => "INFOCONSENT_SELF_TEST_CONSENT_EXPLANATION_TEXT".Translate();

        AuthenticationManager _authManager;
        public event EventHandler<AuthErrorType> OnError;
        public event EventHandler<AuthSuccessType> OnSuccess;

        readonly TemporaryExposureKey.Types.ReportType _reportInfectedType;
        EventHandler<AuthSuccessType> _onSuccess;
        EventHandler<AuthErrorType> _onError;

        public InformationAndConsentViewModel(EventHandler<AuthSuccessType> onSuccess,
            EventHandler<AuthErrorType> onError,
            TemporaryExposureKey.Types.ReportType reportInfectedType)
        {
            _onSuccess = onSuccess;
            _onError = onError;
            _reportInfectedType = reportInfectedType;
            _authManager = new AuthenticationManager();
        }

        public void Init()
        {
            OnError += _onError;
            OnSuccess += _onSuccess;
            try
            {
                _authManager.Setup(OnAuthCompleted, OnAuthError, _reportInfectedType);
            }
            catch (ArgumentException e)
            {
                OnError?.Invoke(this, AuthErrorType.Unknown);
            }
        }

        public void Cleanup()
        {
            if (OnError != null) OnError -= _onError;
            if (OnSuccess != null) OnSuccess -= _onSuccess;
            _onError = null;
            _onSuccess = null;
            if (_authManager != null) _authManager.Cleanup();
        }

        void Unsubscribe()
        {
            if (OnError != null) OnError -= _onError;
            if (OnSuccess != null) OnSuccess -= _onSuccess;
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
#if APPCENTER
            Crashes.TrackError(e.Exception);
#endif
            System.Diagnostics.Debug.Print("Auth errored");
            System.Diagnostics.Debug.Print(e.Message);
            System.Diagnostics.Debug.Print(e.Exception?.ToString());
            OnError?.Invoke(this, AuthErrorType.Unknown);
        }

        void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            string errorMsgPrefix = $"{nameof(InformationAndConsentViewModel)}.{nameof(OnAuthCompleted)}: ";
            System.Diagnostics.Debug.Print("Authenticated: " + e.IsAuthenticated);
            if ((e?.IsAuthenticated ?? false) && e.Account?.Properties != null && e.Account.Properties.ContainsKey("access_token"))
            {
                LogUtils.LogMessage(Enums.LogSeverity.INFO, errorMsgPrefix + "User returned from ID Porten after authentication and access_token exists.");
                AreCountryConsentsGiven = false;

                //Access_token
                string token = e.Account?.Properties["access_token"];
                PersonalDataModel payload = _authManager.GetPayloadValidateJWTToken(token);

                if (payload == null)
                {
                    OnError?.Invoke(this, AuthErrorType.Unknown);
                }
                else
                {
                    //Expiration time
                    if (e.Account.Properties.TryGetValue("expires_in", out string expires))
                    {
                        int.TryParse(expires, out int expiresSeconds);
                        if (expiresSeconds > 0)
                        {
                            payload.TokenExpiration = DateTime.Now.AddSeconds(expiresSeconds);
                            LogUtils.LogMessage(LogSeverity.INFO, $"{errorMsgPrefix} Access-token expires timestamp is {payload.TokenExpiration?.ToString(CultureInfo.InvariantCulture)}");
                        }
                    }
                    else
                    {
                        LogUtils.LogMessage(LogSeverity.ERROR, errorMsgPrefix + "'expires_in' value does not exist");
                    }

                    SaveCovidRelatedAttributes(payload);

                    if (AuthenticationState.PersonalData.IsUnderaged)
                    {
                        OnError?.Invoke(this, AuthErrorType.Underaged);
                        return;
                    }

                    if (AuthenticationState.PersonalData.IsBlocked)
                    {
                        OnError?.Invoke(this, AuthErrorType.MaxTriesExceeded);
                    }
                    else
                    {
                        if (AuthenticationState.PersonalData.CanUploadKeys
                            && !AuthenticationState.PersonalData.UnknownStatus
                            && payload.ValidateAccessToken()
                            )
                        {
                            if (AuthenticationState.PersonalData.IsNotInfected)
                            {
                                if (AuthenticationState.PersonalData.IsMsisLookupSkipped)
                                {
                                    OnSuccess?.Invoke(this, AuthSuccessType.SelfDiagnosis);
                                }
                                else
                                {
                                    OnError?.Invoke(this, AuthErrorType.NotInfected);
                                }
                            }
                            else
                            {
                                if (payload.Validate())
                                {
                                    OnSuccess?.Invoke(this, AuthSuccessType.ConfirmedTest);
                                }
                                else
                                {
                                    OnError?.Invoke(this, AuthErrorType.Unknown);
                                }
                            }
                        }
                        else
                        {
                            if (AuthenticationState.PersonalData.UnknownStatus)
                            {
                                LogUtils.LogMessage(LogSeverity.ERROR, errorMsgPrefix + "Value Covid19_status = ukendt");
                            }
                            OnError?.Invoke(this, AuthErrorType.Unknown);
                        }
                    }
                }
            }
            else
            {
                //The user clicked back
                Restart();
            }
        }

        void Restart()
        {
            Unsubscribe();
            if (_authManager != null) _authManager.Cleanup();
            _authManager = new AuthenticationManager();
            Init();
        }

        void SaveCovidRelatedAttributes(PersonalDataModel payload)
        {
            AuthenticationState.PersonalData = payload;
        }
    }
}
