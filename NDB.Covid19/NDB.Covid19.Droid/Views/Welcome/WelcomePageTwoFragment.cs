using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;

using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePageTwoFragment : Fragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.welcome_page_two, container, false);
            TextView bodyOne = view.FindViewById<TextView>(Resource.Id.welcome_page_two_body_one);
            TextView bodyTwo = view.FindViewById<TextView>(Resource.Id.welcome_page_two_body_two);
            TextView header = view.FindViewById<TextView>(Resource.Id.welcome_page_two_title);

            bodyOne.Text = WelcomeViewModel.WELCOME_PAGE_TWO_BODY_ONE;
            bodyTwo.Text = WelcomeViewModel.WELCOME_PAGE_TWO_BODY_TWO;
            header.Text = WelcomeViewModel.WELCOME_PAGE_TWO_TITLE;

            header.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            view.LayoutDirection = LayoutUtils.GetLayoutDirection();
            Button arrowBack = view.FindViewById<Button>(Resource.Id.arrow_back);
            arrowBack.SetBackgroundResource(LayoutUtils.GetBackArrow());

            WelcomePageTools.SetArrowVisibility(view);

            return view;
        }
    }
}