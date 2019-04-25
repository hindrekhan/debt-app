using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace debt_app
{
    [Activity(Label = "AddDebtActivity")]
    public class AddDebtActivity : AppCompatActivity
    {
        private Spinner contactSpinner;
        private double debtAmount;
        private Intent intent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_debt);
            // spinner stuff
            contactSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            FillSpinner();
            // debtButton stuff
            var debtButton = FindViewById<AppCompatButton>(Resource.Id.btn_addDebt);
            debtButton.Click += delegate { addDebtToDatabase(); };
            // add contact shortcut stuff
            intent = new Intent(this, typeof(AddContactActivity));
            intent.PutExtra("Activity", "First");
            

            var addContact = FindViewById<ImageButton>(Resource.Id.imageButton1);
            addContact.Click += delegate {
                StartActivity(intent);
            };

        }

        private void addDebtToDatabase()
        {
            DatabaseService dbService = new DatabaseService();
            Switch debtSwitch = FindViewById<Switch>(Resource.Id.switch_debt);
            if (contactSpinner.SelectedItem.ToString() == Resources.GetText(Resource.String.no_contacts_available) || contactSpinner.SelectedItem.ToString() == null)
            {
                Toast.MakeText(this, Resources.GetText(Resource.String.err_no_contact_selected), ToastLength.Short).Show();
            }
            else if (FindViewById<EditText>(Resource.Id.editText_debt).Text == "")
            {
                Toast.MakeText(this, Resources.GetText(Resource.String.err_no_debt_amount), ToastLength.Short).Show();
            }
            else
            {
                var people = dbService.GetAllPersons();
                debtAmount = double.Parse(FindViewById<EditText>(Resource.Id.editText_debt).Text);
                Person curPerson = new Person();
                curPerson.Name = contactSpinner.SelectedItem.ToString();
                curPerson.Id = (from contact in people
                                where contact.Name == curPerson.Name
                                select contact.Id).FirstOrDefault();
                curPerson.Email = (from contact in people
                                  where contact.Name == curPerson.Name
                                  select contact.Email).FirstOrDefault();
                curPerson.Debt = (from contact in people
                                  where contact.Name == curPerson.Name
                                  select contact.Debt).FirstOrDefault();
                if (debtSwitch.Checked == true)
                {
                    curPerson.Debt -= debtAmount;
                }
                else
                {
                    curPerson.Debt += debtAmount;
                }
                dbService.UpdatePerson(curPerson);
                people = dbService.GetAllPersons();
                Toast.MakeText(this, Resources.GetText(Resource.String.added_debt_to_db), ToastLength.Short).Show();
                StartActivity(typeof(MainActivity));
            }
        }

        private void FillSpinner()
        {
            DatabaseService dbService = new DatabaseService();
            var people = dbService.GetAllPersons();
            var contacts = from contact in people
                           select contact.Name;
            var contactNames = contacts.ToList();
            
            if (people.Count == 0)
            {
                contactNames.Add(Resources.GetText(Resource.String.no_contacts_available));
                contactSpinner.Enabled = false;
            }
            else
            {
                contactSpinner.Enabled = true;
            }
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.spinner_item, contactNames);
            contactSpinner.Adapter = adapter;
        }
    }
}