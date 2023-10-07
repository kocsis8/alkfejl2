using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passwordManager
{
    public class Option
    {
        [Option('r', "register", HelpText = "Regisztrálni szeretnék")]
        public bool Regist { get; set; }

        [Option('u', "username", HelpText = "Állítsd be a felhaszáló nevet")]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "állítsd be a jelszót")]
        public string Password { get; set; }


        [Option('e', "email", HelpText = "álltsd be az email címet")]
        public string Email { get; set; }


        [Option('f', "firstname", HelpText = "álltsd be a vezeték neved")]
        public string Firstname { get; set; }


        [Option('l', "lastname", HelpText = "állitsd be a kereszt neved")]
        public string Lastname { get; set; }

        //list
        [Option('s', "show", HelpText = "listázd ki az jelszavaid")]
        public bool List { get; set; }


    }
}
