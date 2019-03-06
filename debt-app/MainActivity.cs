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
        ViewPager pager;
        public DatabaseService dbService;
        public Person curPerson = new Person();
        public View firstView;
        public View secondView;

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

                var delete = view.FindViewById<Button>(Resource.Id.deleteButton);
                delete.Click += Delete_Click;

                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.listview_layout, v, false);
                secondView = view;

                var newButton = view.FindViewById<ImageView>(Resource.Id.newButton);
                newButton.Click += NewButton_Click;

                var listView = view.FindViewById<ListView>(Resource.Id.listView);
                listView.Adapter = new PeopleAdapter(this, dbService.GetAllPersons());

                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Person"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "People"));
            pager.SetCurrentItem(1, true);
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

            prices.Text = curPerson.Items;
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

        private void Clear_Click(object sender, System.EventArgs e)
        {
            curPerson.Items = "";
            update_text();
        }

        private void NewButton_Click(object sender, System.EventArgs e)
        {
            pager.SetCurrentItem(0, true);
        }

        private void Item1_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "cucumber ";
            update_text();
        }

        private void Item2_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "apple ";
            update_text();
        }

        private void Item3_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "gum ";
            update_text();
        }

        private void Item4_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "beer ";
            update_text();
        }

        private void Item5_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "cigarettes ";
            update_text();
        }

        private void Item6_Click(object sender, System.EventArgs e)
        {
            curPerson.Items += "vodka ";
            update_text();
        }

        private void update_text()
        {
            finalPrice.Text = "Price: " + Person.CalcDebt(curPerson.Items).ToString();
            prices.Text = curPerson.Items;
        }
    }
}