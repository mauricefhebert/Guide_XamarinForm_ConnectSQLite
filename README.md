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
2. Create a new class representing a table on your database
3. Change the visibility to public
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
