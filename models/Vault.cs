using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace passwordManager.models
{
    internal class Vault
    {
        public static string? userCsvPath { get; set; }

        [Name("user_id")]
        public string? userid { get; set; }
        [Name("username")]
        public string? username { get; set; }
        [Name("password")]
        public string? password { get; set; }
        [Name("website")]
        public string? website { get; set; }

        [Ignore]
        public User user
        {
            get
            {
                if (userCsvPath == null) return null;
                using StreamReader reader = new(userCsvPath);
                using CsvReader csv = new(
                    reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<User>()
                    .Where(el => el.username == userid)
                    .FirstOrDefault();
            }
        }
        public override string ToString()
        {
            return $"Vault: ID = {this.userid}, User_Name = {this.username}, Website = {this.website}, User={user} ";
        }
    }
}

