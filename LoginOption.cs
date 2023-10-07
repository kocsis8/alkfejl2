using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passwordManager
{
    public class LoginOption
    {
        [Option('l', "login", HelpText = "Bejelentkezni szeretnék")]
        public bool Login { get; set; }

        [Option('u', "username", HelpText = "add meg a felhsználó neved")]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "add meg a jelszavad")]
        public string Password { get; set; }

    }
}
