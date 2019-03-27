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
                Toast.MakeText(this, "You are now in debt 9,000,000€ to the Russian Mafia", ToastLength.Short).Show();
                StartActivity(typeof(AddDebtActivity));
            };
            fabContact.Click += (o, e) =>
            {
                CloseFabMenu();
                Toast.MakeText(this, "The Russian Mafia added you on Facebook", ToastLength.Short).Show();
                StartActivity(typeof(AddContactActivity));
            };
            bgFabMenu.Click += (o, e) => CloseFabMenu();
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.AddDebt)
            {
                StartActivity(typeof(AddDebtActivity));
                Toast.MakeText(this, "Write your note and press the button again.", ToastLength.Short).Show();
                return true;
            }
            return base.OnOptionsItemSelected(item);
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
    }
}