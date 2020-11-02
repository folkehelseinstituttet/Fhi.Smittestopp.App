using System.Collections.Generic;
using AndroidX.Fragment.App;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePagerAdapter : FragmentPagerAdapter
    {
        List<AndroidX.Fragment.App.Fragment> pages;

        public WelcomePagerAdapter(AndroidX.Fragment.App.FragmentManager fm, List<AndroidX.Fragment.App.Fragment> pages)
            : base(fm)
        {
            this.pages = pages;
        }

        public override int Count { get { return pages.Count; } }

        public override AndroidX.Fragment.App.Fragment GetItem(int position)
        {         
            return (AndroidX.Fragment.App.Fragment)
                   pages[position];
        }


    }
}