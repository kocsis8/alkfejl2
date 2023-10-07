using CsvHelper;
using System.Globalization;
using passwordManager.models;
using CommandLine;
using passwordManager;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

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

                  if (o.Login)
                  {
                      string username;
                      Console.Write("add meg a felhasználó neved: ");
                      username = Console.ReadLine();

                      string pw;
                      Console.Write("add meg a jelszavad: ");
                      pw = Console.ReadLine();


                      login(username,pw);
                  }

              });

    }

    private static void login(string username, string pw)
    {
        User user = null;

               
                 user = userLogin(username, pw);
               
       

        if (user == null)
        {
            Console.WriteLine("siekrtelen bejelentkezés");
        }
        else 
        {
            Console.WriteLine($"sikeres bejelentkezés: {user.username}");

            string next;
            Console.Write("ird be mit szeretnél (ha a jelszavaidat néznéd meg ird be hogy `lista` ha új jelszót akarsz hozzáadni az alaklamazáshoz ird be hogy `addjelszo`): ");
            next = Console.ReadLine();

            if(next.Equals("lista")) { 
                showAllPw(user);
            }

            if (next.Equals("addjelszo"))
            {
                addnewPw(user);
            }


        }
     }

    private static void addnewPw(User user)
    {
        //user_id,username,password,website
        Vault vault = new Vault();

        vault.userId = user.username;
        Console.Write("add meg a felhasználónevet amit menteni szeretnél: ");
        vault.username = Console.ReadLine();

        string password;
        Console.Write("add meg a menteni kivánt jelszót: ");
        password = Console.ReadLine();

        EncryptedType encryptedData = new EncryptedType(user.email, password);
        EncryptedType encryptedResult = encryptedData.Encrypt();
        vault.password = encryptedResult.Secret;



        Console.Write("add meg a weboldalt amihez mentenéd: ");
        vault.website = Console.ReadLine();


        string vaultCsvPath = Path.Combine("..", "..", "..", "..", "passwordManager", "resources", "db", "vault.csv");

        // append new record at the end of our csv file
        bool mode = true;
        using (StreamWriter writer = new(vaultCsvPath, append: mode))
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using CsvWriter csv = new(writer, config);
            csv.WriteRecords(new Vault[] { vault });
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
    

