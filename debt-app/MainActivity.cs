using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Xamarin.Android.Net;

namespace debt_app
{
    [Activity(Label = "", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        int count = 1;
        EditText prices;
        TextView finalPrice;
        List<string> items = new List<string>();
        ViewPager pager;
        public DatabaseService dbService;
        public Person curPerson = new Person();
        public View firstView;

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

            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.person, v, false);
                firstView = view;

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
                finalPrice = view.FindViewById<TextView>(Resource.Id.finalPrice);

                var clear = view.FindViewById<Button>(Resource.Id.clear);
                clear.Click += Clear_Click;

                var save = view.FindViewById<Button>(Resource.Id.save);
                save.Click += Save_Click;

                var sendBill = view.FindViewById<Button>(Resource.Id.sendBill);
                sendBill.Click += SendBill_Click;

                return view;
            });

            adaptor.AddFragmentView((i, v, b) => {
                var view = i.Inflate(Resource.Layout.listview_layout, v,false);

                var newButton = view.FindViewById<ImageView>(Resource.Id.newButton);
                newButton.Click += NewButton_Click;

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
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "People"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Settings"));
            pager.SetCurrentItem(1, true);
        }

        private void SendBill_Click(object sender, System.EventArgs e)
        {
            Xamarin.Forms.Device.OpenUri(new System.Uri("mailto:ryan.hatfield@test.com"));
        }

        public void UpdatePerson()
        {
            var view = firstView;

            var name = view.FindViewById<EditText>(Resource.Id.name);
            name.Text = curPerson.Name;

        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            dbService.UpdatePerson(curPerson);
        }

        private void Clear_Click(object sender, System.EventArgs e)
        {
            items = new List<string>();
            update_text();
        }

        private void NewButton_Click(object sender, System.EventArgs e)
        {
            pager.SetCurrentItem(0, true);
        }

        private void Item1_Click(object sender, System.EventArgs e)
        {
            items.Add("cucumber");
            update_text();
        }

        private void Item2_Click(object sender, System.EventArgs e)
        {
            items.Add("apple");
            update_text();
        }

        private void Item3_Click(object sender, System.EventArgs e)
        {
            items.Add("gum");
            update_text();
        }

        private void Item4_Click(object sender, System.EventArgs e)
        {
            items.Add("beer");
            update_text();
        }

        private void Item5_Click(object sender, System.EventArgs e)
        {
            items.Add("cigarettes");
            update_text();
        }

        private void Item6_Click(object sender, System.EventArgs e)
        {
            items.Add("vodka");
            update_text();
        }

        private void update_text()
        {
            var finalPrice = 0.0;

            prices.Text = "";
            foreach (var item in items)
            {
                prices.Text += item + " ";

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
                        break;
                }
            }

            this.finalPrice.Text = "Price: " + finalPrice.ToString();
        }
    }
}

