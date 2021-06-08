using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid.Views.Welcome
{
    [Activity(Label = "", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class WelcomeActivity : BaseAppCompatActivity, ViewPager.IOnPageChangeListener
    {
        public bool IsOnBoarding;
        WelcomePageOneFragment _welcomePageOne = new WelcomePageOneFragment();
        WelcomePageTwoFragment _welcomePageTwo = new WelcomePageTwoFragment();
        WelcomePageThreeFragment _welcomePageThree = new WelcomePageThreeFragment();
        WelcomePageFourFragment _welcomePageFour = new WelcomePageFourFragment();
        List<AndroidX.Fragment.App.Fragment> _pages;
        
        Button _button;
        Button _previousButton;
        NonSwipeableViewPager _pager;
        TabLayout _dotLayout;
        int _numPages;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (State(savedInstanceState) == AppState.IsDestroyed)
            {
                return;
            }

            IsOnBoarding = Intent.GetBooleanExtra(DroidRequestCodes.isOnBoardinIntentExtra, false);

            if (IsOnBoarding && ConsentsHelper.IsNotFullyOnboarded)
            {
                GoToConsents();
                return;
            }

            if (IsOnBoarding)
            {
                SetIsBackgroundActivityDialogShowEnableNewUser(true);
            }

            _pages = new List<AndroidX.Fragment.App.Fragment>(new AndroidX.Fragment.App.Fragment[] { _welcomePageOne, _welcomePageTwo, _welcomePageThree, _welcomePageFour });
            _numPages = _pages.Count;

            SetContentView(Resource.Layout.welcome);

            _button = FindViewById<Button>(Resource.Id.buttonGetStarted);
            _previousButton = FindViewById<Button>(Resource.Id.buttonPrev);

            _previousButton.Text = WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT;
            _button.Text = WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT;
            _button.Click += new SingleClick(GetNextButton_Click, 500).Run;
            _previousButton.Click += new SingleClick(GetPreviousButton_Click, 500).Run;
            _previousButton.Visibility = ViewStates.Gone;

            WelcomePagerAdapter adapter = new WelcomePagerAdapter(SupportFragmentManager, _pages);
            _pager = FindViewById<NonSwipeableViewPager>(Resource.Id.fragment);
            _pager.Adapter = adapter;
            _pager.SetPagingEnabled(false);
            _pager.AddOnPageChangeListener(this);
            _pager.AnnounceForAccessibility(IsOnBoarding ? WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE : WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE);

            _dotLayout = FindViewById<TabLayout>(Resource.Id.tabDots);
            _dotLayout.SetupWithViewPager(_pager, true);

            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
        }

        protected override Intent GetStartingNewIntent()
        {
            return NavigationHelper.GetStartPageIntent(this);
        }

        private void GetNextButton_Click(object sender, EventArgs e)
        {
            ScrollToTop();

            if (_numPages == _pager.CurrentItem + 1)
            {
                if (IsOnBoarding)
                {
                    GoToConsents();
                    return;
                }

                RunOnUiThread(Finish);
                return;
            }

            _pager.SetCurrentItem(_pager.CurrentItem + 1, true);

            AnnouncePageChangesForScreenReaders();
        }

        private void GoToConsents()
        {
            Intent intent = new Intent(this, typeof(WelcomePageConsentsActivity));
            StartActivity(intent);
        }

        private void AnnouncePageChangesForScreenReaders()
        {
            // Change focus to fragment
            _pager.PerformAccessibilityAction(Android.Views.Accessibility.Action.AccessibilityFocus, null);

            AndroidX.Fragment.App.Fragment activeFragment = _pages[_pager.CurrentItem];

            if (activeFragment == _welcomePageOne)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE);
            }
            else if (activeFragment == _welcomePageTwo)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_TWO);
            }
            else if (activeFragment == _welcomePageThree)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_THREE);
            }
            else if (activeFragment == _welcomePageFour)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_FOUR);
            }
        }

        private void GetPreviousButton_Click(object sender, EventArgs e)
        {
            ScrollToTop();
            _pager.SetCurrentItem(_pager.CurrentItem - 1, true);
            _button.Visibility = ViewStates.Visible;
            AnnouncePageChangesForScreenReaders();
        }

        public void OnPageScrollStateChanged(int state)
        {
            Console.WriteLine("OnPageScrollStateChanged " + " " + state);
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            Console.WriteLine("OnPageScrolled " + " " + position);
        }

        public void OnPageSelected(int position)
        {
            ScrollToTop();
            _previousButton.Visibility = position == 0 ? ViewStates.Gone : ViewStates.Visible;
        }

        private void ScrollToTop()
        {
            (_pager.Adapter as WelcomePagerAdapter)?.GetItem(_pager.CurrentItem)?.View.ScrollTo(0, 0);
        }
    }
}