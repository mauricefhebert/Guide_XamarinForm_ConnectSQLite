using SQLite;

namespace ConnectSQLiteToXamarinForm.Models
{
    public class Post
    {
        //Create an auto incrementing ID
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        //Fix the length of the string to 250 characters
        [MaxLength(250)]
        public string Experience { get; set; }
    }
}
