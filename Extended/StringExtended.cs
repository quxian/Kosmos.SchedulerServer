using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extended {
    public static class StringExtended {
        public static string GetMD5HashCode(this string str) {
            using (var md5 = MD5.Create()) {
                return string.Join("", md5.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(x => x.ToString("x2")));
            }
        }
    }
}
