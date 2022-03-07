using I18NPortable;

namespace NDB.Covid19.ViewModels
{
    public class SelftestRegistrationViewModel
    {        
        public static string POSITIVE_TEST_MSISBUTTON_TEXT => "CHOOSE_CONFIRMED_TEST_BUTTON_TEXT".Translate();
        public static string POSTIVE_TEST_SELFTESTBUTTON_TEXT => "CHOOSE_SELF_TEST_BUTTON_TEXT".Translate();
        public static string HEADER_LABEL_SELFTEST_REGISTRATION_TEXT => "CHOOSE_TYPE_OF_TEST_HEADER".Translate();

        public SelftestRegistrationViewModel() { }
    }
}
