using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaSach.Utils
{
    public class Helper
    {
        public readonly static string AppKey = "c984aed014aec7623a54f0591da07a85fd4b762d";  //000000 hash SHA1
        public readonly static string Issuer = "mysite.com";

        public static string GenHash(string input)
        {
            return string.Join("", new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))
                .Select(x => x.ToString("X2")).ToArray());
        }
    }
}
