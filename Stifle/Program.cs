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
