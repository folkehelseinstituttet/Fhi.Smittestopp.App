using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.Messages
{
    class MessagesAdapter : BaseAdapter<MessageItemViewModel>
    {
        private Activity _context;
        private List<MessageItemViewModel> _items;

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
            view.FindViewById<TextView>(Resource.Id.messages_item_tile).Text = _items[position].Title;
            view.FindViewById<TextView>(Resource.Id.messages_item_date).Text = _items[position].DayAndMonthString;
            view.FindViewById<TextView>(Resource.Id.messages_item_description).Text = MessageItemViewModel.MESSAGES_RECOMMENDATIONS;
            view.FindViewById<View>(Resource.Id.ellipsis).Visibility =
                _items[position].IsRead
                    ? ViewStates.Invisible
                    : ViewStates.Visible;

            view.SetBackgroundResource(_items[position].IsRead
                ? Resource.Drawable.message_item_normal
                : Resource.Drawable.message_item_pressed);

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