using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NDB.Covid19.Droid.Views.AuthenticationFlow;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.OAuth2
{
    /// <summary>
    /// This Activity is hit when redirecting from ID Porten in the browser
    /// </summary>
    [Activity(
        Label = "AuthUrlSchemeInterceptorActivity",
        LaunchMode = LaunchMode.SingleTop,
        NoHistory = true,
        Name = "md52ecc484fd43c6baf7f3301c3ba1d0d0c.AuthUrlSchemeInterceptorActivity")]
    [
        IntentFilter
    (
        actions: new[] { Intent.ActionView },
        Categories = new[]
                {
                    Intent.CategoryDefault,
                    Intent.CategoryBrowsable
                },
        DataSchemes = new[]
                {
                    "no.fhi.smittestopp-exposure-notification"
                },
        DataPath = "/oauth2redirect"
    )
        ]
    public class AuthUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {

                base.OnCreate(savedInstanceState);

                global::Android.Net.Uri uri_android = Intent.Data;

                // Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
                Uri uri_netfx = new Uri(uri_android.ToString());

                // load redirect_url Page for parsing
                AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

                Intent intent = new Intent(this, typeof(SpinnerPageActivity));
                StartActivity(intent);

                this.Finish();

                return;
            }
            catch (Exception e)
            {

                // Log if Intent is null or Intent.Data is null or Intent.Data
                string error = Intent == null ?
                    "Intent was null" :
                    (Intent.Data == null ? "Intent.Data was null" : "Intent.Data: " + Intent.Data.ToString());

                LogUtils.LogException(Enums.LogSeverity.WARNING, e, nameof(AuthUrlSchemeInterceptorActivity) + " " + nameof(OnCreate) + " error when redirectin to app after ID Porten validation", error);

                // Redirect and hit OnAuthError
                AuthenticationState.Authenticator.OnPageLoading(new Uri("no.fhi.smittestopp:/oauth2redirect"));

                this.Finish();

                return;

            }
        }
    }
}
