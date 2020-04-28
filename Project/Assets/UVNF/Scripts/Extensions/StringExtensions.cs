using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Input can't be empty.");
            return str.First().ToString().ToUpper() + string.Join("", str.Skip(1));
        }
    }
}
