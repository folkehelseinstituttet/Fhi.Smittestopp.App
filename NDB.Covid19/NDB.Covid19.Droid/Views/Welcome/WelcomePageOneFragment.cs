using Android.OS;
using Android.Views;
using Android.Widget;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePageOneFragment : Fragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.welcome_page_one, container, false);
            TextView bodyOne = view.FindViewById<TextView>(Resource.Id.welcome_page_one_body_one);
            TextView bodyTwo = view.FindViewById<TextView>(Resource.Id.welcome_page_one_body_two);
            TextView header = view.FindViewById<TextView>(Resource.Id.welcome_page_one_title);

            bodyOne.Text = WelcomeViewModel.WELCOME_PAGE_ONE_BODY_ONE;
            bodyTwo.Text = WelcomeViewModel.WELCOME_PAGE_ONE_BODY_TWO;
            header.Text = WelcomeViewModel.WELCOME_PAGE_ONE_TITLE;

            WelcomePageTools.SetArrowVisibility(view);

            return view;
        }
    }
}