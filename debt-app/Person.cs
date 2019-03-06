using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace debt_app
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Items { get; set; }
        public string Email { get; set; }

        public static double CalcDebt(string items)
        {
            var debt = 0.0;

            foreach (var item in items.Split())
            {
                switch (item)
                {
                    case "cucumber":
                        debt += 1.0;
                        break;
                    case "gum":
                        debt += 1.0;
                        break;
                    case "beer":
                        debt += 1.0;
                        break;
                    case "cigarettes":
                        debt += 1.0;
                        break;
                    case "vodka":
                        debt += 1.0;
                        break;
                    case "apple":
                        debt += 1.0;
                        break;
                    default:
                        break;
                }
            }

            return debt;
        }
    }
}