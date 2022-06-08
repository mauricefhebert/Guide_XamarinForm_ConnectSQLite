using ConnectSQLiteToXamarinForm.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectSQLiteToXamarinForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadFromDatabase : ContentPage
    {
        public ReadFromDatabase()
        {
            InitializeComponent();
        }

        //When the page load we will read from the database
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Create & Open the connection
            using (SQLiteConnection cn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Create the table if the table doesn't exist, otherwise skip this line
                cn.CreateTable<Post>();

                //Select the table we wanna work with and transform it to a list
                var posts = cn.Table<Post>().ToList();

                //Define the data source for the CollectionView
                postListView.ItemsSource = posts;
            }
            //The connection while be closed here since we use the using statement
        }

        private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            //Get the button
            Button btn = sender as Button;

            //Cast the button binding as Post
            Post selectedPost = btn.BindingContext as Post;
            
            //If selectedPost is not null navigate to update page, otherwise do nothing
            if(selectedPost != null)
            {
                Navigation.PushAsync(new UpdateDatabase(selectedPost));
            }
        }

        private void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            //Get the button
            Button btn = sender as Button;

            //Cast the button binding as Post
            Post selectedPost = btn.BindingContext as Post;

            //If selectedPost is not null proced with the execution, otherwise do nothing
            if (selectedPost != null)
            {

            //Create a new connection to the database the constructor take the Database location that we defined in the App.cs
            using (SQLiteConnection cn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Create a new table of the type of the model's
                cn.CreateTable<Post>();

                //Delete the selectedPost from the database
                int rowsAffected = cn.Delete(selectedPost);

                //Notify the user of success or failure
                if (rowsAffected > 0)
                    DisplayAlert("Success", "Experience successfully delete", "Ok");
                else
                    DisplayAlert("Failure", "Experience failed to be delete", "Ok");
            }
                //The connection while be closed here since we use the using statement
            }
        }
    }
}

//OneWay Binding mode -> The target receive the data from the source he can read it but not modify it
//TwoWay Binding mode -> The target receive the data from the source and can also write to the data source to update it
//OneWayToSource Binding mode -> The target write the data to the source but cannot read it after