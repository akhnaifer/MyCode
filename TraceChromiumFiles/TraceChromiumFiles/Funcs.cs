using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceChromiumFiles
{
    public static class Funcs
    {
        
    
        public static string Between(this string src, string findfrom, string findto)
        {
            int start = src.IndexOf(findfrom)+5;
            int to = src.IndexOf(findto, start + findfrom.Length);
            if (start < 0 || to < 0) return "";
            string s = src.Substring(
                           start + findfrom.Length,
                           to - start - findfrom.Length);
            return s;
        }
    
    }
}
