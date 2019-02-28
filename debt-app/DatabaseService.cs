using System;
using System.Collections.Generic;
using System.IO;
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
    class DatabaseService
    {
        SQLiteConnection db;

        public DatabaseService()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal),
                "mydatabase.db1");

            db = new SQLiteConnection(dbPath);
        }

        public void CreateDatabase()
        {
            db.CreateTable<Person>();
        }

        public void AddPerson(Person Person)
        {
            db.Insert(Person);
        }

        public void UpdatePerson(Person Person)
        {
            db.Update(Person);
        }

        public void CreateTableWithData()
        {
            db.CreateTable<Person>();

            if (db.Table<Person>().Count() == 0)
            {
                var newPerson = new Person();
                newPerson.Name = "jim";
                newPerson.Debt = 1.0f;
                db.Insert(newPerson);
            }
        }

        public List<Person> GetAllPersons()
        {
            var table = db.Table<Person>();

            return table.ToList();
        }

        public void RemovePerson(Person Person)
        {
            db.Delete<Person>(Person);
        }
    }
}