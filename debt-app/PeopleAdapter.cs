using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace debt_app
{
    class PeopleAdapter : BaseAdapter<Person>
    {
        List<Person> items;
        MainActivity context;

        public PeopleAdapter(MainActivity context, List<Person> items) : base()
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

            //if (view == null)
            //{
                
            //        if ( items[position].Debt < 0)
            //        {

            //        view = context.LayoutInflater.Inflate(Resource.Layout.list_item2, null);
            //        }


            //    view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null);
            //}

            //var name = view.FindViewById<TextView>(Resource.Id.txt_name);
            //var debt = view.FindViewById<TextView>(Resource.Id.debt);
            //var layout = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            
            //name.Text = items[position].Name;
            //debt.Text = items[position].Debt.ToString() + "€";
            ////debt.Text = Person.CalcDebt(items[position].Items).ToString() + "€";

            //layout.Tag = position;

           return view;
        }
    }
}