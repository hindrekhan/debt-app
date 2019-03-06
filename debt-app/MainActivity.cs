using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Xamarin.Android.Net;
using System.Linq;
using System;

namespace debt_app
{
    [Activity(Label = "Debt App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        bool trigger = false;
        EditText prices;
        TextView finalPrice;
        ViewPager pager;
        public DatabaseService dbService;
        public Person curPerson = new Person();
        public View firstView;
        public View secondView;
        ViewSwitcher switcher;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Xamarin.Forms.Forms.Init(this, bundle);
            dbService = new DatabaseService();
            dbService.CreateDatabase();
            dbService.CreateTableWithData();

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = firstView = i.Inflate(Resource.Layout.add_debt, v, false);

                switcher = view.FindViewById<ViewSwitcher>(Resource.Id.viewSwitcher_contacts);
                var emailEdit = view.FindViewById<EditText>(Resource.Id.editText_mail);
                emailEdit.FocusChange += new EventHandler<View.FocusChangeEventArgs>(emailEdit_FocusChange);

            var spinner = view.FindViewById<Spinner>(Resource.Id.spinner_contacts);
                Fill_Spinner_Contacts(view, spinner);
                spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {

                var view = i.Inflate(Resource.Layout.listview_layout, v, false);
                secondView = view;

                var listView = view.FindViewById<ListView>(Resource.Id.listView);
                listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());

                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Add Debt"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Contacts"));
            pager.SetCurrentItem(1, true);
        }

        public void Fill_Spinner_Contacts(View view, Spinner spinner)
        {
            DatabaseService dbService = new DatabaseService();
            var people = dbService.GetAllPersons();
            var contacts = from contact in people
                           select contact.Name;
            var contactNames = contacts.ToList();
            contactNames.Add("<Create New Contact>");
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.spinner_item, contactNames);
            spinner.Adapter = adapter;
        }

        private void emailEdit_FocusChange(object sender, EventArgs e)
        {
            Toast.MakeText(Application.Context, "Hello toast!", ToastLength.Short).Show();
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (trigger == true)
            {
                Spinner spinner = (Spinner)sender;
                if (spinner.GetItemAtPosition(e.Position).ToString() == "<Create New Contact>")
                {
                    switcher.ShowNext();
                }
                //string toast = string.Format("The mean temperature for planet ",
                //    spinner.GetItemAtPosition(e.Position));
            }
            trigger = true;
        }

        private void Delete_Click(object sender, System.EventArgs e)
        {
            dbService.RemovePerson(curPerson);
            pager.SetCurrentItem(1, true);
            UpdatePeople();
            UpdatePerson();
        }

        public void UpdatePeople()
        {
            var listView = secondView.FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());
        }

        private void SendBill_Click(object sender, System.EventArgs e)
        {
            Xamarin.Forms.Device.OpenUri(new System.Uri("mailto:" + curPerson.Name + "@mail.com"));
        }

        public void UpdatePerson()
        {
            var view = firstView;

            var name = view.FindViewById<EditText>(Resource.Id.name);
            name.Text = curPerson.Name;

        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            var name = firstView.FindViewById<EditText>(Resource.Id.name);
            curPerson.Name = name.Text;

            if (curPerson.Id == 0)
                dbService.AddPerson(curPerson);
            else
                dbService.UpdatePerson(curPerson);

            curPerson = new Person();
            UpdatePeople();
            UpdatePerson();
            pager.SetCurrentItem(1, true);
        }

        
    }
}