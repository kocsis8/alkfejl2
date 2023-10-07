using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System;

namespace passwordManager.models
{
    internal class Vault
    {
        //user_id,username,password,website

        public static string? userCsvPath { get; set; }

        [Name("user_id")]
        public string userId { get; set; }

        [Name("username")]
        public string username { get; set; }

        [Name("password")]
        public string password { get; set; }

        [Name("website")]
        public string website { get; set; }

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
                    .Where(el => el.username == userId)
                    .FirstOrDefault();
            }
        }

        public override string ToString()
        {
            return $"Vault: ID = {this.userId}, User_Name = {this.username}, Website = {this.website}, User={user} "; 
        }
    }
}
