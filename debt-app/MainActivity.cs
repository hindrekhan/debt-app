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
using Android.Views.InputMethods;
using Android.Hardware.Input;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

namespace debt_app
{
    [Activity(Label = "Debt App", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity
    {
        ViewPager pager;
        public DatabaseService dbService;
        public Person curPerson = new Person();
        public View firstView;
        public View secondView;
        ViewSwitcher switcher;
        RelativeLayout debtLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppCenter.Start("0f77a2e6-05ad-4dc9-a831-725683dc2e64",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("0f77a2e6-05ad-4dc9-a831-725683dc2e64", typeof(Analytics), typeof(Crashes));
            Distribute.SetEnabledAsync(true);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Xamarin.Forms.Forms.Init(this, bundle);
            dbService = new DatabaseService();
            dbService.CreateDatabase();
            dbService.CreateTableWithData();
            dbService.DeleteDatabase();
            RefreshViews();
        }

        private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Send)
            {
                InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                e.Handled = true;
            }
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
            var what = switcher.IndexOfChild(firstView);
            debtLayout.Visibility = ViewStates.Visible;
            var sendBill = FindViewById<Button>(Resource.Id.button_finish);
            sendBill.Click += SendBill_Click;

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
                Spinner spinner = (Spinner)sender;
                if (spinner.GetItemAtPosition(e.Position).ToString() == "<Create New Contact>")
                {
                    switcher.ShowNext();
                    firstView.FindViewById<EditText>(Resource.Id.editText_name).Text = "";
                    firstView.FindViewById<EditText>(Resource.Id.editText_mail).Text = "";
                }
                else
                {
                    debtLayout.Visibility = ViewStates.Visible;
                    var sendBill = FindViewById<Button>(Resource.Id.button_finish);
                    sendBill.Click += SendBill_Click;
                }
        }

        private int GetIndex(Spinner spinner, String myString)
        {
            for (int i = 0; i < spinner.Count; i++)
            {
                if (spinner.GetItemAtPosition(i).ToString().Equals(myString))
                {
                    return i;
                }
            }

            return -1;
        }

        private void OnClick_Add_Contact(object sender, EventArgs e)
        {
            Save_Click(sender, e);
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

            var spinner = firstView.FindViewById<Spinner>(Resource.Id.spinner_contacts);
            Fill_Spinner_Contacts(firstView, spinner);
        }

        private void SendBill_Click(object sender, System.EventArgs e)
        {
            var numberEdit = FindViewById<EditText>(Resource.Id.editText_value);
            Save_Click(sender, e);
            //Xamarin.Forms.Device.OpenUri(new System.Uri("mailto:" + curPerson.Email));
        }

        public void UpdatePerson()
        {
            var spinner = firstView.FindViewById<Spinner>(Resource.Id.spinner_contacts);
            spinner.SetSelection(GetIndex(spinner, curPerson.Name));

            
            //var name = firstView.FindViewById<EditText>(Resource.Id.editText_name);
            //name.Text = curPerson.Name;
        }

        public void RefreshViews()
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = firstView = i.Inflate(Resource.Layout.add_debt, v, false);

                switcher = view.FindViewById<ViewSwitcher>(Resource.Id.viewSwitcher_contacts);

                var emailEdit = view.FindViewById<EditText>(Resource.Id.editText_mail);
                emailEdit.FocusChange += new EventHandler<View.FocusChangeEventArgs>(emailEdit_FocusChange);

                var numberEdit = view.FindViewById<EditText>(Resource.Id.editText_value);
                numberEdit.EditorAction += HandleEditorAction;

                debtLayout = view.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_debt);

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

            ActionBar.RemoveAllTabs();
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Add Debt"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Contacts"));
            pager.SetCurrentItem(1, true);
        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            var people = dbService.GetAllPersons();
            var spinner = firstView.FindViewById<Spinner>(Resource.Id.spinner_contacts);
            string name;
            string email = "abc123@gmail.com";
            EditText editText_name = firstView.FindViewById<EditText>(Resource.Id.editText_name);
            if (editText_name.Text == "")
            {
                name = spinner.SelectedItem.ToString();
                if (name == "" || name == "<Create New Contact>")
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Warning");
                    alert.SetMessage("You entered incorrect name");
                    alert.Show();
                    return;
                }
            }
            else
            {
                name = editText_name.Text;
            }

            double debt = 0.0;
            try
            {
                debt = double.Parse(FindViewById<EditText>(Resource.Id.editText_value).Text);
            }
            catch
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Warning");
                alert.SetMessage("You entered incorrect debt amount");
                alert.Show();
                return;
            }

            curPerson.Name = name;
            curPerson.Email = email;
            if (firstView.FindViewById<Switch>(Resource.Id.switch_debt).Checked==true)
            {
                curPerson.Debt -= debt;
            }
            else
            {
                curPerson.Debt += debt;
            }
            


            //var contacts = (from contact in people
            //                where contact.Name == curPerson.Name
            //               select contact).FirstOrDefault;

            if (!people.Any(s => s.Name == curPerson.Name))
            {
                dbService.AddPerson(curPerson);
            }

            else
            {
                dbService.UpdatePerson(curPerson);
            }


            curPerson = new Person();
            UpdatePeople();
            UpdatePerson();
            pager.SetCurrentItem(1, true);
            RefreshViews();
            
        }
    }
}