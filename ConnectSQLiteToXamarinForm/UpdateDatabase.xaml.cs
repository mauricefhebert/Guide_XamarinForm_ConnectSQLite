using ConnectSQLiteToXamarinForm.Models;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectSQLiteToXamarinForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDatabase : ContentPage
    {
        //Create a variable
        Post selectedPost;
        public UpdateDatabase(Post selectedPost)
        {
            InitializeComponent();
            //Assign the object passed as parameter to the variable
            this.selectedPost = selectedPost;

            //Add text from selectedPost properties to the entry
            this.experienceEntry.Text = selectedPost.Experience;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            selectedPost.Experience = experienceEntry.Text;

            //Create a new connection to the database the constructor take the Database location that we defined in the App.cs
            using (SQLiteConnection cn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Create a new table of the type of the model's
                cn.CreateTable<Post>();

                //Update the database with the object used as parameter
                int rowsAffected = cn.Update(selectedPost);

                //Notify the user of success or failure
                if (rowsAffected > 0)
                    DisplayAlert("Success", "Experience successfully updated", "Ok");
                else
                    DisplayAlert("Failure", "Experience failed to be updated", "Ok");
            }
            //The connection while be closed here since we use the using statement
        }
    }
}