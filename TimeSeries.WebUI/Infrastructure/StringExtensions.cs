using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TimeSeries.WebUI.Infrastructure
{
    static class StringExtensions
    {
        // Converting double array to string
        public static string ToDoubleString(this double[] s)
        {
            string result = "";
            if (s!=null&&s.Length>0)
            {
                foreach (var item in s)
                {
                    if (!String.IsNullOrWhiteSpace(result))
                        result += ",";
                    result += item.ToString().Replace(",", ".");
                }
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
                number = Regex.Split(s, @"\s");
            }
            return number;
        }

        // Converting to double
        public static double ToDouble(this string s)
        {
            double number = 0;
            if (!String.IsNullOrWhiteSpace(s))
            {
                s = s.Replace(".", ",").Trim();
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
                int sLength = s.Length;
                for (int i = 0; i < sLength; i++)
                {
                    if (!String.IsNullOrWhiteSpace(s[i]))
                        numbers.Add(s[i].ToDouble());
                }
            }
            return numbers.ToArray();
        }
    }
}