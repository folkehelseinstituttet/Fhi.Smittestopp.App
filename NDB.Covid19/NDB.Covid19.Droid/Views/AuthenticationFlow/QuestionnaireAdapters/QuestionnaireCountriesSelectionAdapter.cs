using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow.QuestionnaireAdapters
{
    class QuestionnaireCountriesSelectionAdapter : RecyclerView.Adapter
    {
        private List<CountryDetailsViewModel> _countryList;
        private List<CountryDetailsViewModel> Data
        {
            get => _countryList;
            set
            {
                _countryList = value;
                NotifyDataSetChanged();
            }
        }
        private readonly List<int> _selectedItems = new List<int>();

        public QuestionnaireCountriesSelectionAdapter(List<CountryDetailsViewModel> countryList)
        {
            _countryList = countryList;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CountryDetailsViewModel item = Data[position];
            if (holder is QuestionnaireCountriesSelectionAdapterViewHolder viewHolder)
            {
                viewHolder.Caption.Text = item.Name;
                viewHolder.Check.Checked = _selectedItems.Contains(position);

                viewHolder.SetOnClickListener(new CheckedChangeListener(this, holder, position));
            }
        }

        private class CheckedChangeListener : Java.Lang.Object, View.IOnClickListener
        {
            private readonly RecyclerView.ViewHolder _holder;
            private readonly QuestionnaireCountriesSelectionAdapter _self;
            private readonly int _position;

            public CheckedChangeListener(QuestionnaireCountriesSelectionAdapter self, RecyclerView.ViewHolder holder, int position)
            {
                _holder = holder;
                _self = self;
                _position = position;
            }

            public void OnClick(View v)
            {
                QuestionnaireCountriesSelectionAdapterViewHolder vh = (QuestionnaireCountriesSelectionAdapterViewHolder) _holder;
                vh.Check.Checked = !vh.Check.Checked;
                _self.Data[_position].Checked = vh.Check.Checked;
            }
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            ConstraintLayout view = LayoutInflater
                .From(parent.Context)
                .Inflate(Resource.Layout.country_view, parent, false) as ConstraintLayout;
            return new QuestionnaireCountriesSelectionAdapterViewHolder(view);
        }

        public override int ItemCount => Data.Count;
    }

    class QuestionnaireCountriesSelectionAdapterViewHolder : RecyclerView.ViewHolder
    {
        public CheckBox Check { get; }
        public TextView Caption { get; }

        private readonly View _itemView;

        public QuestionnaireCountriesSelectionAdapterViewHolder(View item) : base(item)
        {
            _itemView = item;

            Check = item.FindViewById<CheckBox>(Resource.Id.country_item_checkbox);
            Caption = item.FindViewById<TextView>(Resource.Id.country_item_caption);

            item.LayoutDirection = LayoutUtils.GetLayoutDirection();

            Check.Clickable = false;
        }

        public void SetOnClickListener(View.IOnClickListener onClickListener)
        {
            _itemView.SetOnClickListener(onClickListener);
        }
    }
}