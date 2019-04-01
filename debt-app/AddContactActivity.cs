using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace debt_app
{
    [Activity(Label = "Activity1")]
    public class AddContactActivity : Activity
    {
        DatabaseService dbService = new DatabaseService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_contact);
            var btnSaveContact = FindViewById<AppCompatButton>(2131296397);
            btnSaveContact.Click += delegate { addContactToDatabase(); };

        }
        private void addContactToDatabase()
        {
            Person curPerson = new Person();
            curPerson.Name = FindViewById<EditText>(Resource.Id.editText_name).Text;
            curPerson.Email = FindViewById<EditText>(Resource.Id.editText_email).Text;
            curPerson.Debt = 0;
            // Prevent double names for Contacts
            var people = dbService.GetAllPersons();
            bool has = people.Any(cus => cus.Name.Trim() == curPerson.Name.Trim());
            if (has == true)
            {
                Toast.MakeText(this, "Error: Name exists already", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Contact saved", ToastLength.Short).Show();
                dbService.AddPerson(curPerson);
                // Get intent, because using shortcut to add contact should return to add debt activity
                var extra = Intent.GetStringExtra("Activity");
                if (extra == "First")
                {
                    StartActivity(typeof(AddDebtActivity));
                }
                else
                {
                    StartActivity(typeof(MainActivity));
                }
            }
            
        }
    }
}