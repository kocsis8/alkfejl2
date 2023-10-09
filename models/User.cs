using CsvHelper.Configuration.Attributes;

namespace passwordManager.models
{
    internal class User
    {
        [Name("username")]
        public string? username { get; set; }
        [Name("password")]
        public string? password { get; set; }
        [Name("email")]
        public string? email { get; set; }
        [Name("firstname")]
        public string? firstname { get; set; }
        [Name("lastname")]
        public string? lastname { get; set; }

        public override string ToString()
        {
            return $"User: User_Name = {this.username}, Email = {this.email}, First_name = {this.firstname}, Last_Name = {this.lastname} ";
        }
    }
    
}
