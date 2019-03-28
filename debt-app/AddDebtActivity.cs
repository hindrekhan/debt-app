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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_debt);
            contactSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            var debtButton = FindViewById<AppCompatButton>(Resource.Id.btn_addDebt);
            FillSpinner();
            debtButton.Click += delegate { addDebtToDatabase(); };
        }

        private void addDebtToDatabase()
        {
            DatabaseService dbService = new DatabaseService();
            Switch debtSwitch = FindViewById<Switch>(Resource.Id.switch_debt);
            if (contactSpinner.SelectedItem.ToString() == "No Contacts Available" || contactSpinner.SelectedItem.ToString() == null)
            {
                Toast.MakeText(this, "Error: No contact selected, create contact", ToastLength.Short).Show();
            }
            else if (FindViewById<EditText>(Resource.Id.editText_debt).Text == "")
            {
                Toast.MakeText(this, "Error: No debt amount entered", ToastLength.Short).Show();
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
                Toast.MakeText(this, "Debt added to database", ToastLength.Short).Show();
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
                contactNames.Add("No Contacts Available");
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