using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stifle.Modules
{
    public class ArgParse
    {
        public static void Help()
        {
            string help =
                "Stifle.exe [Operations] [Options]\n" +
                "Operations:\n" +
                "";
            Console.WriteLine(help);
        }

        static List<string> sMods = new List<string>
        {
            "list",
            "add",
            "remove",
            "clear"

        };

        public static Dictionary<string, string> Parse(string[] args, string[] flags, string[] options)
        {
            Dictionary<string, string> cmd = new Dictionary<string, string>();

            foreach (string flag in flags)
            {
                if (args.Contains(flag))
                {
                    try
                    {
                        cmd.Add(flag, args[Array.IndexOf(args, flag) + 1]);
                    }
                    catch
                    {
                        Console.WriteLine("[!] Please supply all the valid options, use \"Stifle.exe -h\" for more information");
                        return null;
                    }
                }
            }

            foreach (string option in options)
            {
                if (args.Contains(option))
                {
                    cmd.Add(option, "True");
                }
                else
                {
                    cmd.Add(option, "False");
                }
            }

            return cmd;
        }

        public static void Execute(string[] args)
        {
            if (args.Contains("--help") || args.Contains("-h") || args.Length == 0)
            {
                Help();
            }
            else if (args.Length > 0)
            {
                string principal = null;
                string add = null;
                string password = null;
                string pfx = null;

                string[] flags = { "--object", "--pfx", "--password" };
                string[] options = { "--add" };

                Dictionary<string, string> cmd = Parse(args, flags, options);
                if (cmd == null)
                {
                    return;
                }

                cmd.TryGetValue("--object", out principal);
                cmd.TryGetValue("--add", out add);
                cmd.TryGetValue("--pfx", out pfx);
                cmd.TryGetValue("--password", out password);
                

                if (add != null)
                {
                    byte[] rawcert = Convert.FromBase64String(pfx);
                    Operations.Add(principal, rawcert, password);
                }
            }

        }
    }
}
