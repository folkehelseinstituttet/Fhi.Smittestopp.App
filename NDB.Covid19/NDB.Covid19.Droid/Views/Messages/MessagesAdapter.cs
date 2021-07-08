using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using I18NPortable;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.Messages
{
    class MessagesAdapter : BaseAdapter<MessageItemViewModel>
    {
        private readonly Activity _context;
        private readonly List<MessageItemViewModel> _items;

        public MessagesAdapter(Activity context, MessageItemViewModel[] items)
        {
            _context = context;
            _items = items.ToList();
        }
        public void AddItems(List<MessageItemViewModel> messages)
        {
            _items.AddRange(messages);
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.messages_list_element, null);
            TextView title = view.FindViewById<TextView>(Resource.Id.messages_item_title);
            TextView dateView = view.FindViewById<TextView>(Resource.Id.messages_item_date);
            title.Text = _items[position].Title.Translate();
            title.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            view.FindViewById<TextView>(Resource.Id.new_item).Text = MessagesViewModel.MESSAGES_NEW_ITEM;
            dateView.Text = _items[position].DayAndMonthString;
            dateView.TextAlignment = TextAlignment.ViewStart;
            view.FindViewById<TextView>(Resource.Id.messages_item_description).Text = MessageItemViewModel.MESSAGES_RECOMMENDATIONS;
            view.FindViewById<LinearLayout>(Resource.Id.dot_layout).Visibility =
                _items[position].IsRead
                    ? ViewStates.Gone
                    : ViewStates.Visible;

            parent.LayoutDirection = LayoutUtils.GetLayoutDirection();

            return view;
        }

        public void ClearList()
        {
            _items.Clear();
            NotifyDataSetChanged();
        }

        public override MessageItemViewModel this[int position] => _items[position];

        public override int Count => _items.Count;
    }
}