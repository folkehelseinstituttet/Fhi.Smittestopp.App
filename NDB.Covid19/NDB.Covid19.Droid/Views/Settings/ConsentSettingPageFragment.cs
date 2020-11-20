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

        TextView _aboutHeader;
        TextView _aboutText1;
        TextView _aboutText2;
        TextView _howitworksHeader;
        TextView _howitworksText1;
        TextView _howitworksText2;
        TextView _samtykkeText1;
        TextView _samtykkeText2;
        TextView _behandlingafpersonoplysningerText;
        TextView _samtykkebottomHeader;
        TextView _samtykkebottomText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.consent_settings_page_body, container, false);

            RelativeLayout RelativeLayout4 = view.FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvordan_accepterer);
            TextView consentFourTitle = RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_title);
            RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FOUR_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);

            //ABOUT
            _aboutHeader = view.FindViewById<TextView>(Resource.Id.consent1_about_header);
            _aboutHeader.Text = ConsentViewModel.CONSENT_ONE_TITLE;
            _aboutText1 = view.FindViewById<TextView>(Resource.Id.consent1_about_text_section1);
            _aboutText1.Text = ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_ONE;
            _aboutText2 = view.FindViewById<TextView>(Resource.Id.consent1_about_text_section2);
            _aboutText2.Text = ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_TWO;

            //HOW IT WORKS
            _howitworksHeader = view.FindViewById<TextView>(Resource.Id.consent1_howitworks_header);
            _howitworksHeader.Text = ConsentViewModel.CONSENT_TWO_TITLE;
            _howitworksText1 = view.FindViewById<TextView>(Resource.Id.consent1_howitworks_text_section1);
            _howitworksText1.Text = ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_ONE;
            _howitworksText2 = view.FindViewById<TextView>(Resource.Id.consent1_howitworks_text_section2);
            _howitworksText2.Text = ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_TWO;

            //SAMTYKKE
            _samtykkeText1 = view.FindViewById<TextView>(Resource.Id.consent1_samtykke_text_section1);
            _samtykkeText1.Text = ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_ONE;
            _samtykkeText2 = view.FindViewById<TextView>(Resource.Id.consent1_samtykke_text_section2);
            _samtykkeText2.Text = ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_TWO;

            //BEHANDLING AF PERSONOPLYSNINGWER
            _behandlingafpersonoplysningerText = view.FindViewById<TextView>(Resource.Id.consent1_behandlingafpersonopl_text);
            _behandlingafpersonoplysningerText.Text = ConsentViewModel.CONSENT_FIVE_PARAGRAPH;

            //SAMTYKKE, BOTTOM
            _samtykkebottomHeader = view.FindViewById<TextView>(Resource.Id.consent1_samtykkebottom_header);
            _samtykkebottomHeader.Text = ConsentViewModel.CONSENT_SIX_TITLE;
            _samtykkebottomText = view.FindViewById<TextView>(Resource.Id.consent1_samtykkebottom_text);
            _samtykkebottomText.Text = ConsentViewModel.CONSENT_SIX_PARAGRAPH;


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
