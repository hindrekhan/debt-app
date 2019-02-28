using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace debt_app
{
    class PeopleAdapter : BaseAdapter<Person>
    {
        List<Person> items;
        Activity context;

        public PeopleAdapter(Activity context, List<Person> items) : base()
        {
            this.context = context;
            this.items = items;
        }


        public override Person this[int position]
        {
            get { return items[position]; }
        }

        public override int Count { get { return items.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null);
            }

            var name = view.FindViewById<TextView>(Resource.Id.txt_name);
            var text = view.FindViewById<TextView>(Resource.Id.debt);

            name.Text = items[position].Name;
            text.Text = items[position].Debt.ToString();

            return view;
        }
    }
}