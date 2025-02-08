using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stifle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("   _____ _   _  __ _      \r\n  / ____| | (_)/ _| |     \r\n | (___ | |_ _| |_| | ___ \r\n  \\___ \\| __| |  _| |/ _ \\\r\n  ____) | |_| | | | |  __/\r\n |_____/ \\__|_|_| |_|\\___|\n\n [*] Exploit explicit certificate mapping in Active Directory\n");
            try
            {
                Modules.ArgParse.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Exception: " + e.Message);
            }
        }
    }
}
