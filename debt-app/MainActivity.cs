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
        EditText prices;
        TextView finalPrice;

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
                var view = i.Inflate(Resource.Layout.person, v, false);

                var item1 = view.FindViewById<ImageView>(Resource.Id.item1);
                item1.Click += Item1_Click;

                var item2 = view.FindViewById<ImageView>(Resource.Id.item2);
                item2.Click += Item2_Click;

                var item3 = view.FindViewById<ImageView>(Resource.Id.item3);
                item3.Click += Item3_Click;

                var item4 = view.FindViewById<ImageView>(Resource.Id.item4);
                item4.Click += Item4_Click;

                var item5 = view.FindViewById<ImageView>(Resource.Id.item5);
                item5.Click += Item5_Click;

                var item6 = view.FindViewById<ImageView>(Resource.Id.item6);
                item6.Click += Item6_Click;

                prices = view.FindViewById<EditText>(Resource.Id.prices);

                //var listView = view.FindViewById<ListView>(Resource.Id.listView);
                //listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());

                return view;
            });

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

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Person"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Asdf"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "People"));
        }

        private void Item1_Click(object sender, System.EventArgs e)
        {
            prices.Text += "cucumber ";
            update_text();
        }

        private void Item2_Click(object sender, System.EventArgs e)
        {
            prices.Text += "apple ";
            update_text();
        }

        private void Item3_Click(object sender, System.EventArgs e)
        {
            prices.Text += "gum ";
            update_text();
        }

        private void Item4_Click(object sender, System.EventArgs e)
        {
            prices.Text += "beer ";
            update_text();
        }

        private void Item5_Click(object sender, System.EventArgs e)
        {
            prices.Text += "cigarettes ";
            update_text();
        }

        private void Item6_Click(object sender, System.EventArgs e)
        {
            prices.Text += "vodka ";
            update_text();
        }

        private void update_text()
        {
            var items = prices.Text.Split();
            var finalPrice = 0.0;
            foreach (var item in items)
            {
                if (item == "")
                    continue;

                switch(item)
                {
                    case "cucumber":
                        finalPrice += 1.0;
                        break;
                    case "gum":
                        finalPrice += 1.0;
                        break;
                    case "beer":
                        finalPrice += 1.0;
                        break;
                    case "cigarettes":
                        finalPrice += 1.0;
                        break;
                    case "vodka":
                        finalPrice += 1.0;
                        break;
                    case "apple":
                        finalPrice += 1.0;
                        break;
                    default:
                        finalPrice = -2.0;
                        break;
                }

                if (finalPrice == -2.0)
                {
                    prices.Text = "Error";
                    break;
                }
            }

            prices.Text = finalPrice.ToString();
        }
    }
}

