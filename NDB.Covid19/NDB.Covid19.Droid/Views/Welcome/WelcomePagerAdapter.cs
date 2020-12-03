using System.Collections.Generic;
using AndroidX.Fragment.App;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePagerAdapter : FragmentPagerAdapter
    {
        List<Fragment> pages;

        public WelcomePagerAdapter(FragmentManager fm, List<Fragment> pages) : base(fm, BehaviorResumeOnlyCurrentFragment)
        {
            this.pages = pages;
        }

        public override int Count { get { return pages.Count; } }

        public override Fragment GetItem(int position)
        {         
            return pages[position];
        }


    }
}