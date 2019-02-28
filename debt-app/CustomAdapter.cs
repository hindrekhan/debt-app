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
using debt_app;
using Java.Lang;

namespace debt_app
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        
    }
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<User> users;

        public CustomAdapter(Activity activity,List<User> users)
        {
            this.activity = activity;
            this.users = users;
        }
        public override int Count
        {
            get
            {
                return users.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return users[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
           var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_item, parent,false);

            var txtName = view.FindViewById<TextView>(Resource.Id.txt_name);

            txtName.Text = users[position].Name;

            return view;

        }
    }
}