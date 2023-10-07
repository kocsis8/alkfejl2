using CsvHelper;
using System.Globalization;
using passwordManager.models;
using CommandLine;
using passwordManager;
using CsvHelper.Configuration;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {


        string workDir = null;

        // Parancssori argumentumok feldolgozása
        foreach (var arg in args)
        {
            if (arg.StartsWith("--workdir="))
            {
                // Az argumentumban megadott munkakönyvtár
                workDir = arg.Substring("--workdir=".Length);
            }
        }

        if (string.IsNullOrWhiteSpace(workDir))
        {
            // Ha nincs megadva munkakönyvtár, akkor használjuk az aktuális munkakönyvtárat
            workDir = Environment.CurrentDirectory;
        }
       

        Parser.Default.ParseArguments<Option>(args)
              .WithParsed<Option>(o =>
              {
                  if (o.Regist)
                  {
                      RegisterUser(args);
                  }

                  if (o.List)
                  {

                      showPw(args);
                  }

              });

    }

    private static void showPw(string[] args)
    {
        User user = null;

        Parser.Default.ParseArguments<Option>(args)
               .WithParsed<Option>(o => { 
               
                 user = userLogin(o.Username, o.Password);
               
               });

        if (user == null)
        {
            Console.WriteLine("siekrtelen bejelentkezés");
        }
        else 
        {
            Console.WriteLine($"sikeres bejelentkezés: {user.username}");
            showAllPw(user);
        }
     }

    private static void showAllPw(User user)
    {
        Vault.userCsvPath = Path.Combine("..", "..", "..", "..", "passwordManager", "resources", "db", "user.csv");
        string vaultCsvPath = Path.Combine("..", "..", "..", "..", "passwordManager", "resources", "db", "vault.csv");

        using (StreamReader reader = new(vaultCsvPath))
        {
            using CsvReader csv = new(
                reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Vault>().ToList();

            foreach (var record in records)
            {
                if (record.userId.Equals(user.username))
                {
                    Console.WriteLine(record);
                }
            }
        }
    }

    private static void RegisterUser(string[] args)
    {
        Parser.Default.ParseArguments<RegisterOption>(args)
              .WithParsed<RegisterOption>(o =>
              {
                  if (o.Regist)
                  {

                      User user = new User();
                      user.username = o.Username;

                      EncryptedType encryptedData = new EncryptedType(o.Email, o.Password);
                      EncryptedType encryptedResult = encryptedData.Encrypt();
                      user.password = encryptedResult.Secret;

                      user.email = o.Email;
                      user.firstname = o.Firstname;
                      user.lastname = o.Lastname;
                      addUser(user);
                  }

              });
    }

    private static User userLogin(string username, string password)
    {
        User user = null;

    string userCsvPath = Path.Combine("..", "..", "..", "..", "passwordManager", "resources", "db", "user.csv");

    using (StreamReader reader = new(userCsvPath))
    {
        using CsvReader csv = new(
            reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<User>().ToList();

        foreach (var record in records)

        {
                if (record.username == (username))
                {

                   
                    EncryptedType encryptedData = new EncryptedType(record.email, record.password);
                    EncryptedType decryptedResult = encryptedData.Decrypt();


                    if (password == decryptedResult.Secret)
                    {

                        user = new User();
                        user.username = record.username;
                        user.password = record.password;
                        user.email = record.email;
                        user.firstname = record.firstname;
                        user.lastname = record.lastname;

                        return user;

                    }
                }
        }
    }

        return user;
    }

    private static void addUser(User user) { 

        string userCsvPath = Path.Combine("..", "..", "..", "..", "passwordManager", "resources", "db", "user.csv");
    
        // append new record at the end of our csv file
        bool mode = true;
        using (StreamWriter writer = new(userCsvPath, append: mode))
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using CsvWriter csv = new(writer, config);
            csv.WriteRecords(new User[] { user });
        }
    }


}
    

