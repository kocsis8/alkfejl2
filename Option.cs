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
         
        [Option('l', "login", HelpText = "bejelentkezés")]
        public bool Login { get; set; }


    }
}
