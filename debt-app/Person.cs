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
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(70)]
        public string Name { get; set; }
        public double Debt { get; set; }
        [MaxLength(89)]
        public string Email { get; set; }

    }
}