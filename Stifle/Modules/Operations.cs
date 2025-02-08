using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.DirectoryServices;

namespace Stifle.Modules
{
    internal class Operations
    {
        public static bool Add(string prinicpal, byte[] rawcert, string password)
        {

            X509Certificate2 cert = new X509Certificate2(rawcert, password, X509KeyStorageFlags.PersistKeySet);

            string altSecurityIdentities = Cert.GetAltSecurityIdentities(cert);
            Console.WriteLine(" [*] Derived altSecurityIdentities value: " + altSecurityIdentities);
            setASI(altSecurityIdentities, prinicpal);

            return true;
        }

        public static void setASI(string altSecurityIdentities, string user)
        {
            try
            {
                SearchResultCollection results;

                DirectoryEntry de = new DirectoryEntry();
                DirectorySearcher ds = new DirectorySearcher(de);

                string query = "(samaccountname=" + user + ")";
                ds.Filter = query;
                results = ds.FindAll();

                if (results.Count == 0)
                {
                    Console.WriteLine(" [!] Cannot find account");
                    return;
                }

                foreach (SearchResult sr in results)
                {
                    DirectoryEntry mde = sr.GetDirectoryEntry();
                    mde.Properties["altSecurityIdentities"].Add(altSecurityIdentities);
                    mde.CommitChanges();
                    Console.WriteLine(" [+] Certificate mapping added to " + user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" [!] Failed to write altSecurityIdentities attrbute: " + ex.Message);
            }
        }

        public static void removeASI(string user)
        {
            SearchResultCollection results;

            DirectoryEntry de = new DirectoryEntry();
            DirectorySearcher ds = new DirectorySearcher(de);

            string query = "(samaccountname=" + user + ")";
            ds.Filter = query;
            results = ds.FindAll();

            if (results.Count == 0)
            {
                Console.WriteLine("[!] Cannot find account");
                return;
            }

            foreach (SearchResult sr in results)
            {
                DirectoryEntry mde = sr.GetDirectoryEntry();
                mde.Properties["altSecurityIdentities"].Clear();    
                mde.CommitChanges();
                Console.WriteLine(" [+] Certificate mapping cleared from " + user);
            }
        }
    }
}
