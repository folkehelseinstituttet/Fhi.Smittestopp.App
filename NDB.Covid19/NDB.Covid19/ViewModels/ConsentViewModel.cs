using System;
using System.Collections.Generic;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using static NDB.Covid19.Models.ConsentViewModel;

namespace NDB.Covid19.ViewModels
{
    public class ConsentViewModel
    {
        //CONSENT PAGE TEXTS
        public static string WELCOME_PAGE_CONSENT_TITLE => "WELCOME_PAGE_FIVE_TITLE".Translate();
        public static string CONSENT_ONE_TITLE => "CONSENT_ONE_TITLE".Translate();
        public static string CONSENT_ONE_PARAGRAPH => "CONSENT_ONE_PARAGRAPH".Translate();
        public static string CONSENT_TWO_TITLE => "CONSENT_TWO_TITLE".Translate();
        public static string CONSENT_TWO_PARAGRAPH => "CONSENT_TWO_PARAGRAPH".Translate();
        public static string CONSENT_THREE_TITLE => "CONSENT_THREE_TITLE".Translate();
        public static string CONSENT_THREE_PARAGRAPH => "CONSENT_THREE_PARAGRAPH".Translate();
        public static string CONSENT_FOUR_TITLE => "CONSENT_FOUR_TITLE".Translate();
        public static string CONSENT_FOUR_PARAGRAPH => "CONSENT_FOUR_PARAGRAPH".Translate();
        public static string CONSENT_FIVE_TITLE => "CONSENT_FIVE_TITLE".Translate();
        public static string CONSENT_FIVE_PARAGRAPH => "CONSENT_FIVE_PARAGRAPH".Translate();
        public static string CONSENT_SIX_TITLE => "CONSENT_SIX_TITLE".Translate();
        public static string CONSENT_SIX_PARAGRAPH => "CONSENT_SIX_PARAGRAPH".Translate();
        public static string CONSENT_SEVEN_TITLE => "CONSENT_SEVEN_TITLE".Translate();
        public static string CONSENT_SEVEN_PARAGRAPH => "CONSENT_SEVEN_PARAGRAPH".Translate();
        public static string CONSENT_SEVEN_BUTTON_TEXT => "CONSENT_SEVEN_BUTTON_TEXT".Translate();
        public static string CONSENT_SEVEN_BUTTON_URL => "CONSENT_SEVEN_BUTTON_URL".Translate();
        public static string CONSENT_EIGHT_TITLE => "CONSENT_EIGHT_TITLE".Translate();
        public static string CONSENT_EIGHT_PARAGRAPH => "CONSENT_EIGHT_PARAGRAPH".Translate();
        public static string CONSENT_NINE_TITLE => "CONSENT_NINE_TITLE".Translate();
        public static string CONSENT_NINE_PARAGRAPH => "CONSENT_NINE_PARAGRAPH".Translate();

        public static string CONSENT_REMOVE_TITLE => "CONSENT_REMOVE_TITLE".Translate();
        public static string CONSENT_REMOVE_MESSAGE => "CONSENT_REMOVE_MESSAGE".Translate();
        public static string CONSENT_OK_BUTTON_TEXT => "CONSENT_OK_BUTTON_TEXT".Translate();
        public static string CONSENT_NO_BUTTON_TEXT => "CONSENT_NO_BUTTON_TEXT".Translate();

        public static string GIVE_CONSENT_TEXT => "CONSENT_GIVE_CONSENT".Translate();

        public static string WITHDRAW_CONSENT_BUTTON_TEXT => "CONSENT_WITHDRAW_BUTTON_TEXT".Translate();
        public static string WITHDRAW_CONSENT_SUCCESS_TITLE => "CONSENT_WITHDRAW_SUCCES_TITLE".Translate();
        public static string WITHDRAW_CONSENT_SUCCESS_TEXT => "CONSENT_WITHDRAW_SUCCES_BODY".Translate();

        public static string SWITCH_ACCESSIBILITY_CONSENT_SWITCH_DESCRIPTOR => "WELCOME_PAGE_FIVE_ACCESSIBILITY_CONSENT_SWITCH".Translate();
        public static string SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_GIVEN => "WELCOME_PAGE_FIVE_SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_GIVEN".Translate();
        public static string SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_NOT_GIVEN => "WELCOME_PAGE_FIVE_SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_NOT_GIVEN".Translate();
        public static string CONSENT_THREE_PARAGRAPH_ACCESSIBILITY => "CONSENT_THREE_PARAGRAPH_ACCESSIBILITY".Translate();
        public static string CONSENT_REQUIRED => "CONSENT_REQUIRED".Translate();
        public bool ConsentIsGiven = false;

        /// <summary>
        /// Opens the privacy policy in an in-app browser.
        /// </summary>
        public static void OpenPrivacyPolicyLink()
        {
            try
            {
                ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(CONSENT_SEVEN_BUTTON_URL);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "Failed to open Privacy policy");
            }
        }

        /// <returns>The list of texts needed in the consents section in the right order</returns>
        public List<ConsentSectionTexts> GetConsentSectionsTexts()
        {
            return new List<ConsentSectionTexts>
            {
                new ConsentSectionTexts(CONSENT_ONE_TITLE, CONSENT_ONE_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_TWO_TITLE, CONSENT_TWO_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_THREE_TITLE, CONSENT_THREE_PARAGRAPH, CONSENT_THREE_PARAGRAPH_ACCESSIBILITY),
                new ConsentSectionTexts(CONSENT_FOUR_TITLE, CONSENT_FOUR_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_FIVE_TITLE, CONSENT_FIVE_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_SIX_TITLE, CONSENT_SIX_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_SEVEN_TITLE, CONSENT_SEVEN_PARAGRAPH, CONSENT_SEVEN_PARAGRAPH.Replace("|", "")),
                new ConsentSectionTexts(CONSENT_EIGHT_TITLE, CONSENT_EIGHT_PARAGRAPH, null),
                new ConsentSectionTexts(CONSENT_NINE_TITLE, CONSENT_NINE_PARAGRAPH, null)
            };
        }

    }
}
