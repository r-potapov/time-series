using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace TimeSeries.WebUI.Infrastructure
{
    static class StringExtensions
    {
        // Converting double array to string
        public static string ToDoubleString(this double[] s)
        {
            string result = "";
            if (s != null && s.Length > 0)
            {
                result = String.Join(" ", s);
            }
            return result.Trim();
        }
        // Converting to string array
        public static string[] ToStringArray(this string s)
        {
            string[] number = new string[0];
            if (!String.IsNullOrWhiteSpace(s))
            {
                s = Regex.Replace(s, "[\r\n|\n]", " ", RegexOptions.Compiled);
                s = Regex.Replace(s, @"[\s+]", " ", RegexOptions.Compiled);
                number = Regex.Split(s, @"\s").Where(p => p.Length > 0).ToArray();
            }
            return number;
        }

        // Converting to double
        public static double ToDouble(this string s)
        {
            var newCulture = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            newCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCulture;

            double number = 0;
            if (!String.IsNullOrWhiteSpace(s))
            {
                s = s.Replace(",", ".").Trim();
                double.TryParse(s, out number);
            }
            return number;
        }

        // Converting string array to double array
        public static double[] ToDouble(this string[] s)
        {
            List<double> numbers = new List<double>();
            if (s != null && s.Length > 0)
            {
                //int sLength = s.Length;
                //for (int i = 0; i < sLength; i++)
                //{
                //    if (!String.IsNullOrWhiteSpace(s[i]))
                //        numbers.Add(s[i].ToDouble());
                //}
                numbers = s.Select(x => x.ToDouble()).ToList();
            }
            return numbers.ToArray();
        }
    }
}