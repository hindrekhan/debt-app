using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using debt_app;

namespace AndroidPager
{
    [Activity(Label = "AndroidPager", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.listview_layout, v,false);
                //var textSample = view.FindViewById<TextView>(Resource.Id.txtText);
                //textSample.Text = "This is first page";
                return view;
            });

            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.listview_layout,v, false);
                //var textSample = view.FindViewById<TextView>(Resource.Id.txtText);
                //textSample.Text = "This is second page";
                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "First Tab"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Second Tab"));
        }
    }
}

