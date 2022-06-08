using ConnectSQLiteToXamarinForm.Models;
using SQLite;
using System;
using Xamarin.Forms;

namespace ConnectSQLiteToXamarinForm
{
    public partial class InsertIntoDatabase : ContentPage
    {
        public InsertIntoDatabase()
        {
            InitializeComponent();
        }

        private void Btn_Submit_Clicked(object sender, EventArgs e)
        {
            //Create the object we want to insert into the database following the model's properties except for the ID since it auto incremented
            Post post = new Post()
            {
                Experience = this.experienceEntry.Text
            };

            //Create a new connection to the database the constructor take the Database location that we defined in the App.cs
            using (SQLiteConnection cn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Create a new table of the type of the model's
                cn.CreateTable<Post>();

                //Insert into the database the parameter is the object we want to insert
                int rowsAffected = cn.Insert(post);

                //Notify the user of success or failure
                if (rowsAffected > 0)
                    DisplayAlert("Success", "Experience successfully inserted", "Ok");
                else
                    DisplayAlert("Failure", "Experience failed to be inserted", "Ok");
            }
            //The connection while be closed here since we use the using statement

        }

        private void Btn_Cancel_Clicked(object sender, EventArgs e)
        {
            //Reset the Entry to an empty string
            experienceEntry.Text = string.Empty;
        }
    }
}
