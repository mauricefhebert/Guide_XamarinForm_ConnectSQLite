# ConnectSQLiteToXamarinForm

## 1. Add the sqlite-net-pcl NuGet Package to the solution

`Install-Package sqlite-net-pcl`

## 2. Edit the Constructor for each system

### Xamarin App.cs

```C#
    //Add the following properties to app.cs
    public static string DatabaseLocation = string.Empty;

    //Add this constructor to overwrite the default one don't replace it
    public App(string databaseLocation)
    {
        InitializeComponent();
        MainPage = new MainPage();
        DatabaseLocation = databaseLocation;
    }
```

### Android MainActivity.cs

```C#
    //Above LoadApplication(new App()); Add the following code

    //Define the Database name
    string dbName = "name_db.sqlite";

    //Define the folder where the database will be store on the device
    string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

    //Combine the string
    string fullPath = Path.Combine(folderPath, dbName);

    //Add the fullPath variable as a parameter to LoadApplication(new App());
    LoadApplication(new App(fullPath));
```

### IOS AppDelegate.cs

```C#
    //Above LoadApplication(new App()); Add the following code

    //Define the Database name
    string dbName = "name_db.sqlite";

    //Define the folder where the database will be store on the device
    string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");

    //Combine the string
    string fullPath = Path.Combine(folderPath, dbName);

    //Add the fullPath variable as a parameter to LoadApplication(new App());
    LoadApplication(new App(fullPath));
```

## 3. Create a model

1. Create a Models folder in your main project
2. Create a new class inside the models folder representing a table of your database
3. Change the visibility of the class to public
4. Define the needed properties
5. Add `using SQLite;`
6. Define the SQLite attributes for each properties

   [See more SQLite attributes](https://docs.microsoft.com/en-us/xamarin/android/data-cloud/data-access/using-sqlite-orm#sqlite-attributes)

### Example

```C#
using SQLite;

namespace ConnectSQLiteToXamarinForm.Models
{
    internal class Post
    {
        //Create an auto incrementing ID
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        //Fix the length of the string to 250 characters
        [MaxLength(250)]
        public string Experience { get; set; }
    }
}
```

## 4. Insert data in the database

1. Create a form in the page where you want the user to enter some data
2. Create add an event to the form submit button
3. Inside the event create a new object of the type you wanna add
4. Create a new SQLite connection object
5. The SQLite connection object take as parameter the connection string we defined in the App.cs
6. Create the table for the type of object
7. Use the insert method to insert in the data as parameter use the new created object you wanna insert
8. Close the connection
9. Notify the user of a success or a failure

### XAML

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="ConnectSQLiteToXamarinForm.InsertIntoDatabase"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <StackLayout Margin="10">
        <Label
            FontSize="20"
            Text="Please enter your experience"
            TextColor="Black" />
        <Entry x:Name="experienceEntry" Placeholder="Experience" />
        <StackLayout HorizontalOptions="CenterAndExpand" Orientation="Horizontal">
            <Button Clicked="Btn_Cancel_Clicked" Text="Cancel" />
            <Button Clicked="Btn_Submit_Clicked" Text="Submit" />
        </StackLayout>
    </StackLayout>
</ContentPage>
```

### Code Behind

```C#
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

```

## 5. Read data from the database

1. Create a the UI to receive the data
2. In the code behind override the OnAppearing method
3. Inside the method open the connection
4. Create the table
5. Store the table in a variable as a list
6. Assign the variable as the data source for the UI

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="ConnectSQLiteToXamarinForm.ReadFromDatabase"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <!--  Add a x:Name to the CollectionView to be able to use it in the code behind  -->
    <CollectionView x:Name="postListView">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <!--  Bind the properties we wanna display  -->
                <Label Text="{Binding Experience}" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

```C#
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

        //OneWay Binding mode -> The target receive the data from the source he can read it but not modify it
        //TwoWay Binding mode -> The target receive the data from the source and can also write to the data source to update it
        //OneWayToSource Binding mode -> The target write the data to the source but cannot read it after
```

## 6. Update data in the database

1. Select how you wanna delete the item from the database i choose a button
2. Get the button and cast it as a button object
3. Get the object from the button cast it as the object type

### XML of the ListView

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="ConnectSQLiteToXamarinForm.ReadFromDatabase"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <!--  Add a x:Name to the CollectionView to be able to use it in the code behind  -->
    <CollectionView x:Name="postListView" SelectionMode="None">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Orientation="Horizontal">
                    <!--  Bind the properties we wanna display  -->
                    <Label
                        FontSize="20"
                        HorizontalOptions="StartAndExpand"
                        Text="{Binding Experience}"
                        VerticalTextAlignment="Center" />
                    <Button
                        BackgroundColor="LightGreen"
                        Clicked="Btn_Update_Clicked"
                        HorizontalOptions="CenterAndExpand"
                        Text="Update" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

### Code behind of the ListView for the update button

```C#
private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            //Get the button
            Button btn = sender as Button;

            //Cast the button binding as Post
            Post selectedPost = btn.BindingContext as Post;

            //If selectedPost is not null navigate to update page, otherwise do nothing
            if(selectedPost != null)
            {
                //Navigate to the update page and past the selected object as a parameter
                Navigation.PushAsync(new UpdateDatabase(selectedPost));
            }
        }
```

### The UpdateDatabase View

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="ConnectSQLiteToXamarinForm.UpdateDatabase"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <ContentPage.Content>
        <StackLayout>
            <Entry x:Name="experienceEntry" />
            <Button
                x:Name="updateButton"
                Clicked="UpdateButton_Clicked"
                Text="Update" />
        </StackLayout>
    </ContentPage.Content>
</ContentPage>
```

### Code behind of the UpdateDatabase View

```C#
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
```

## 7. Delete from the database

1. Select how you wanna delete the object from the database i choose a button
2. Get the button and cast it as a Button Object
3. Get the post from the button and cast it as the Object you want to delete
4. If the selection is not null execute the code

### The View of the delete button

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
    x:Class="ConnectSQLiteToXamarinForm.ReadFromDatabase"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <!--  Add a x:Name to the CollectionView to be able to use it in the code behind  -->
    <CollectionView x:Name="postListView" SelectionMode="None">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Orientation="Horizontal">
                    <!--  Bind the properties we wanna display  -->
                    <Label
                        FontSize="20"
                        HorizontalOptions="StartAndExpand"
                        Text="{Binding Experience}"
                        VerticalTextAlignment="Center" />
                    <Button
                        BackgroundColor="OrangeRed"
                        Clicked="Btn_Delete_Clicked"
                        HorizontalOptions="EndAndExpand"
                        Text="Delete" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

### Code behind of the delete method

```C#
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
```
