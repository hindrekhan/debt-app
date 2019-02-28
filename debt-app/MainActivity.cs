using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using debt_app;
using Android.Content;

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

            DatabaseService dbService = new DatabaseService();
            dbService.CreateDatabase();
            dbService.CreateTableWithData();

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.listview_layout, v,false);

                var listView = view.FindViewById<ListView>(Resource.Id.listView);
                listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());

                return view;
            });

            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.layout1,v, false);
                //var listView = view.FindViewById<ListView>(Resource.Id.listView);
                //listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());
                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Asdf"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "People"));
        }
    }
}

