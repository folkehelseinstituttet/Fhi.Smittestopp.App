using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Text;
using AndroidX.Fragment.App;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.Settings
{
    public class ConsentSettingPageFragment : Fragment
    {
        public event EventHandler<Boolean> ButtonPressed;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.consent_settings_page_body, container, false);

            //get all consent paragraph layouts
            RelativeLayout RelativeLayout1 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_frivillig_brug);
            RelativeLayout RelativeLayout2 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_sadan_fungerer_appen);
            RelativeLayout RelativeLayout3 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvad_registreres);
            RelativeLayout RelativeLayout4 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvordan_accepterer);
            RelativeLayout RelativeLayout5 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_ret);
            RelativeLayout RelativeLayout6 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_kontaktregistringer);
            RelativeLayout RelativeLayout7 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_mere);
            RelativeLayout RelativeLayout8 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_behandlingen);
            RelativeLayout RelativeLayout9 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_aendringer);

            TextView consentOneTitle = RelativeLayout1.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentTwoTitle = RelativeLayout2.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentThreeTitle = RelativeLayout3.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentFourTitle = RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentFiveTitle = RelativeLayout5.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentSixTitle = RelativeLayout6.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentSevenTitle = RelativeLayout7.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentEightTitle = RelativeLayout8.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentNineTitle = RelativeLayout9.FindViewById<TextView>(Resource.Id.consent_page_title);

            consentOneTitle.Text = ConsentViewModel.CONSENT_ONE_TITLE;
            consentTwoTitle.Text = ConsentViewModel.CONSENT_TWO_TITLE;
            consentThreeTitle.Text = ConsentViewModel.CONSENT_THREE_TITLE;
            consentFourTitle.Text = ConsentViewModel.CONSENT_FOUR_TITLE;
            consentFiveTitle.Text = ConsentViewModel.CONSENT_FIVE_TITLE;
            consentSixTitle.Text = ConsentViewModel.CONSENT_SIX_TITLE;
            consentSevenTitle.Text = ConsentViewModel.CONSENT_SEVEN_TITLE;
            consentEightTitle.Text = ConsentViewModel.CONSENT_EIGHT_TITLE;
            consentNineTitle.Text = ConsentViewModel.CONSENT_NINE_TITLE;

            consentOneTitle.ContentDescription = ConsentViewModel.CONSENT_ONE_TITLE.ToLower();
            consentTwoTitle.ContentDescription = ConsentViewModel.CONSENT_TWO_TITLE.ToLower();
            consentThreeTitle.ContentDescription = ConsentViewModel.CONSENT_THREE_TITLE.ToLower();
            consentFourTitle.ContentDescription = ConsentViewModel.CONSENT_FOUR_TITLE.ToLower();
            consentFiveTitle.ContentDescription = ConsentViewModel.CONSENT_FIVE_TITLE.ToLower();
            consentSixTitle.ContentDescription = ConsentViewModel.CONSENT_SIX_TITLE.ToLower();
            consentSevenTitle.ContentDescription = ConsentViewModel.CONSENT_SEVEN_TITLE.ToLower();
            consentEightTitle.ContentDescription = ConsentViewModel.CONSENT_EIGHT_TITLE.ToLower();
            consentNineTitle.ContentDescription = ConsentViewModel.CONSENT_NINE_TITLE.ToLower();

            RelativeLayout1.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_ONE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout2.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_TWO_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout3.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_THREE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FOUR_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout5.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FIVE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout6.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_SIX_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout7.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_SEVEN_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout8.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_EIGHT_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout9.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_NINE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);

            Button policyLinkBtn = view.FindViewById<Button>(Resource.Id.consent_paragraph_policy_btn);
            policyLinkBtn.Text = ConsentViewModel.CONSENT_SEVEN_BUTTON_TEXT;
            policyLinkBtn.Click += PolicyLinkBtn_Click;

            return view;
        }

        private void PolicyLinkBtn_Click(object sender, EventArgs e)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            EventHandler<Boolean> handler = ButtonPressed;
            handler?.Invoke(this, e.IsChecked);
        }
    }
}
