using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePageThreeFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.welcome_page_four, container, false);
            TextView bodyOne = view.FindViewById<TextView>(Resource.Id.welcome_page_four_body_one);
            TextView bodyTwo = view.FindViewById<TextView>(Resource.Id.welcome_page_four_body_two);
            TextView bodyThree = view.FindViewById<TextView>(Resource.Id.welcome_page_four_body_three);
            TextView header = view.FindViewById<TextView>(Resource.Id.welcome_page_four_title);
            bodyOne.Text = WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_ONE;
            bodyTwo.Text = WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_TWO;
            bodyThree.Text = WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_THREE;
            header.Text = WelcomeViewModel.WELCOME_PAGE_FOUR_TITLE;

            header.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            view.LayoutDirection = LayoutUtils.GetLayoutDirection();
            Button arrowBack = view.FindViewById<Button>(Resource.Id.arrow_back);
            arrowBack.SetBackgroundResource(LayoutUtils.GetBackArrow());

            WelcomePageTools.SetArrowVisibility(view);

            return view;
        }
    }
}