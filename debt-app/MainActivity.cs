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

                inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                e.Handled = true;
            }
        }

        private void Delete_Click(object sender, System.EventArgs e)
        {
            dbService.RemovePerson(curPerson);
            pager.SetCurrentItem(1, true);
            UpdatePeople();
        }

        public void UpdatePeople()
        {
            var listView = secondView.FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());
        }


        public void RefreshViews()
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = firstView = i.Inflate(Resource.Layout.add_debt, v, false);
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
    }
}