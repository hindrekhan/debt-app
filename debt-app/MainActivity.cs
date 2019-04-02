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
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Animation;
using System.IO;

namespace debt_app
{
    [Activity(Label = "Debt App", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private static bool isFabOpen;
        private FloatingActionButton fabDebt;
        private FloatingActionButton fabContact;
        private FloatingActionButton fabMain;
        private View bgFabMenu;
        //public Person curPerson = new Person();
        //public View firstView;
        //public View secondView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppCenter.Start("0f77a2e6-05ad-4dc9-a831-725683dc2e64",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("0f77a2e6-05ad-4dc9-a831-725683dc2e64", typeof(Analytics), typeof(Crashes));
            Distribute.SetEnabledAsync(true);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            if (!File.Exists(Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal),
                "debtDB2.db1")))
            {
                var db = new DatabaseService();
                db.CreateDatabase();
            }
            else
            {
                LoadListView();
            }
            
            fabDebt = FindViewById<FloatingActionButton>(Resource.Id.fab_debt);
            fabContact = FindViewById<FloatingActionButton>(Resource.Id.fab_contact);
            fabMain = FindViewById<FloatingActionButton>(Resource.Id.fab_main);
            bgFabMenu = FindViewById<View>(Resource.Id.bg_fab_menu);

            fabMain.Click += (o, e) =>
            {
                if (!isFabOpen)
                {
                    ShowFabMenu();
                }
                else
                {
                    CloseFabMenu();
                }
            };

            fabDebt.Click += (o, e) =>
            {
                CloseFabMenu();
                StartActivity(typeof(AddDebtActivity));
            };
            fabContact.Click += (o, e) =>
            {
                CloseFabMenu();
                StartActivity(typeof(AddContactActivity));
            };
            bgFabMenu.Click += (o, e) => CloseFabMenu();
        }

        public void LoadListView()
        {
            var dbService = new DatabaseService();
            var listView = FindViewById<ListView>(Resource.Id.listView1);
            var people = dbService.GetAllPersons();
            try
            {
                listView.Adapter = new PeopleAdapter(this, RemoveZeros(people));
                listView.ItemClick += ListView_ItemClick;
            }
            catch (Exception)
            {
            }
            
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var layout = (View)sender;
            var pos = (int)layout.Tag;
            var dbService = new DatabaseService();

            var people = dbService.GetAllPersons();
            var curPerson = people[pos];

            var listView = FindViewById<ListView>(Resource.Id.listView1);
            var menu = new PopupMenu(this, listView.GetChildAt(e.Position));
            menu.Inflate(Resource.Menu.menu_popup);
            menu.MenuItemClick += (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.pop_button1:
                        dbService.RemovePerson(curPerson);
                        LoadListView();
                        break;

                }
            };
            menu.Show();
        }

        private List<Person> RemoveZeros(List<Person> people)
        {
            var zeros = (from contact in people
                         where contact.Debt != 0
                         select contact).ToList();
            return zeros;
        }

        private void ShowFabMenu()
        {
            isFabOpen = true;
            fabDebt.Visibility = ViewStates.Visible;
            fabContact.Visibility = ViewStates.Visible;
            bgFabMenu.Visibility = ViewStates.Visible;

            fabMain.Animate().Rotation(135f);
            bgFabMenu.Animate().Alpha(1f);
            fabDebt.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_135))
                .Rotation(0f);
            fabContact.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_75))
                .Rotation(0f);
        }

        private void CloseFabMenu()
        {
            isFabOpen = false;
            fabMain.Animate().Rotation(0f);
            bgFabMenu.Animate().Alpha(0f);
            fabDebt.Animate()
                .TranslationY(0f)
                .Rotation(90f);
            fabContact.Animate()
                .TranslationY(0f)
                .Rotation(90f).SetListener(new FabAnimatorListener(bgFabMenu, fabContact, fabDebt));
        }

        private class FabAnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
        {
            View[] viewsToHide;

            public FabAnimatorListener(params View[] viewsToHide)
            {
                this.viewsToHide = viewsToHide;
            }

            public void OnAnimationCancel(Animator animation) { }

            public void OnAnimationEnd(Animator animation)
            {
                if (!isFabOpen)
                {
                    foreach (var view in viewsToHide)
                    {
                        view.Visibility = ViewStates.Gone;
                    }
                }
            }

            public void OnAnimationRepeat(Animator animation) { }

            public void OnAnimationStart(Animator animation) { }
        }
    }
}