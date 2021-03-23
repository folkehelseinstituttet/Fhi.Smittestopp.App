using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePageFourFragment : Fragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.welcome_page_three, container, false);
            TextView bodyOne = view.FindViewById<TextView>(Resource.Id.welcome_page_three_body_one);
            TextView bodyTwo = view.FindViewById<TextView>(Resource.Id.welcome_page_three_body_two);
            TextView header = view.FindViewById<TextView>(Resource.Id.welcome_page_three_title);
            TextView infoBoxBody = view.FindViewById<TextView>(Resource.Id.welcome_page_three_infobox_body);

            bodyOne.Text = WelcomeViewModel.WELCOME_PAGE_THREE_BODY_ONE;
            bodyTwo.Text = WelcomeViewModel.WELCOME_PAGE_THREE_BODY_TWO;
            header.Text = WelcomeViewModel.WELCOME_PAGE_THREE_TITLE;
            infoBoxBody.Text = WelcomeViewModel.WELCOME_PAGE_THREE_INFOBOX_BODY;

            header.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            infoBoxBody.ContentDescription = WelcomeViewModel.WELCOME_PAGE_THREE_INFOBOX_BODY;

            view.LayoutDirection = LayoutUtils.GetLayoutDirection();
            Button arrowBack = view.FindViewById<Button>(Resource.Id.arrow_back);
            arrowBack.SetBackgroundResource(LayoutUtils.GetBackArrow());

            WelcomePageTools.SetArrowVisibility(view);

            return view;
        }
    }
}