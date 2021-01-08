using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public static class StringDateTimeFormatExtensions
    {
        public static DateTime ParseMyFormatDateTime(this string s)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return DateTime.ParseExact(s, "MM/dd/yyyy hh:mm:ss", culture);
        }
        public static DateTime StringtoDateTime(this string s)
        {
            //var culture = System.Globalization.CultureInfo.CurrentCulture;
            return Convert.ToDateTime(s);
        }
        public static string DateTimetoString(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string CleanString(this string s)
        {
            string val = "";
            val = s.Trim('"');
            return val.Trim();

        }
        public static string FormatTime(this string inputTime)
        {
            string outputTime = string.Empty;
            string timeFormat = inputTime.Substring(inputTime.Length - 2);
            switch (timeFormat)
            {
                case ("AM"):
                    outputTime = inputTime.Replace("AM", "");
                    break;
                case ("PM"):
                    outputTime = inputTime.Replace("PM", "");
                    break;
            }
            return outputTime;
        }
       public static string getMonthAbbreviatedName(this DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day);

            return date.ToString("MMM");
        }

        // function to get the full month name 
       public static string getMonthFullName(this DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day);

            return date.ToString("MMMM");
        }
        public static void WriteCustomString(string  file,string text)
        {

            using (StreamWriter writer = new StreamWriter(file))
            {
    
                    writer.WriteLine(text);
                
            }


        }

    }
}
