using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passwordManager
{
    public class ListOtion
    {
        [Option('l', "list", HelpText = "listázd ki az jelszavaid")]
        public bool List { get; set; }

        [Option('f', "filter", HelpText = "addj meg egy webhelyet aminek a jelszavát szeretnéd")]
        public string Filter { get; set; }
    }
}
