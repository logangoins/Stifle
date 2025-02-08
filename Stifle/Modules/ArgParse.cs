using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Stifle.Modules
{
    public class ArgParse
    {
        public static void Help()
        {
            string help =
                " [*] Add an explicit certificate mapping on a target object by writing the required altSecurityIdentities value using a certificate and the certificate password:\n" +
                "\tStifle.exe add /object:target /certificate:MIIMrQI... /password:P@ssw0rd\n\n" +
                " [*] Clear the altSecurityIdentities attribute, removing the explicit certificate mapping:\n" +
                "\tStifle.exe clear /object:target\n";
            Console.WriteLine(help);
        }

        static List<string> sMods = new List<string>
        {
            "add",
            "clear",

        };

        // Adapted from Certify https://github.com/GhostPack/Rubeus/blob/master/Rubeus/Domain/ArgumentParser.cs#L8
        public static Dictionary<string, string> Parse(IEnumerable<string> args)
        {
            var arguments = new Dictionary<string, string>();
            try
            {
                foreach (var argument in args)
                {
                    var idx = argument.IndexOf(':');
                    if (idx > 0)
                    {
                        arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                    }
                    else
                    {
                        idx = argument.IndexOf('=');
                        if (idx > 0)
                        {
                            arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                        }
                        else
                        {
                            arguments[argument] = string.Empty;
                        }
                    }
                }

                return arguments;
            }
            catch
            {
                Console.WriteLine("[!] Error parsing arguments");
                return null;
            }
        }

        public static void Execute(string[] args)
        {
            if(args.Length == 0)
            {
                Help();
            }
            else if (sMods.Contains(args.First()))
            {
                try
                {
                    switch (args.First().ToLower())
                    {
                        case "add":
                            if (args.Length > 1)
                            {

                                string principal = null;
                                string password = null;
                                string pfx = null;

                                Dictionary<string, string> cmd = Parse(args);

                                if (cmd == null)
                                {
                                    return;
                                }

                                cmd.TryGetValue("/object", out principal);
                                cmd.TryGetValue("/certificate", out pfx);
                                cmd.TryGetValue("/password", out password);

                                if (principal == null || pfx == null || password == null)
                                {
                                    Console.WriteLine("[!] To add an explicit certificate mapping: the object, certificate, and password for the certificate are required");
                                }
                                else
                                {
                                    byte[] rawcert = Convert.FromBase64String(pfx);
                                    Operations.Add(principal, rawcert, password);
                                }
                            }
                            else
                            {
                                Help();
                            }
                            break;
                        case "clear":
                            if (args.Length > 1)
                            {
                                string principal = null;

                                Dictionary<string , string> cmd = Parse(args);
                                if (cmd == null)
                                {
                                    return;
                                }

                                cmd.TryGetValue("/object", out principal);

                                if (principal == null)
                                {
                                    Console.WriteLine("[!] Please supply an object to remove the altSecurityIdentities attribute on");
                                }
                                else
                                {
                                    Operations.removeASI(principal);
                                }

                            }
                            break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("[!] Command invalid");
                }

            }
            else
            {
                Help();
            }
        }
    }
}
