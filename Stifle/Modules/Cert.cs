using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Stifle.Modules
{
    public class Cert
    {

        // Credit to Dave Cossa (@G0ldenGunSec)
        public static string GetAltSecurityIdentities(X509Certificate2 cert)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();

            string serialNumberString = cert.SerialNumber.ToString();
            for (int index = 0; index < serialNumberString.Length; index += 2)
            {
                string str3 = serialNumberString.Substring(serialNumberString.Length - (index + 2), 2);
                stringBuilder1.Append(str3);
            }
            string[] strArray = cert.Issuer.Split(',');
            for (int index = strArray.Length - 1; index >= 0; --index)
            {
                stringBuilder2.Append(strArray[index] + ",");
            }
            --stringBuilder2.Length;
            string dnStr = stringBuilder2.ToString();
            dnStr = dnStr.Replace(" ", "");
            string altSecuritIdentities = String.Format("X509:<I>{0}<SR>{1}", dnStr, (object)stringBuilder1.ToString());

            return altSecuritIdentities;
        }
    }
}
