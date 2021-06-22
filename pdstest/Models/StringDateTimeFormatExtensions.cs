using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace pdstest.Models
{
    public static class StringDateTimeFormatExtensions
    {
        private static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public static DateTime ParseMyFormatDateTime(this string s)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return DateTime.ParseExact(s, "MM/dd/yyyy hh:mm:ss", culture);
        }
        public static DateTime GetIndianDateTimeNow(this DateTime dt)
        {
            dt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            return dt;
        }
        public static DateTime StringtoDateTime(this string s)
        {
            //var culture = System.Globalization.CultureInfo.CurrentCulture;
            return Convert.ToDateTime(s);
        }
        public static string DateTimetoString(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public static string DateTimetoStringforView(this DateTime d)
        {
            return d.ToString("MM/dd/yyyy");
        }
        public static string StringDateTimetoStringView(this string dt)
        {
            DateTime d = DateTime.Now.GetIndianDateTimeNow();
            if (!string.IsNullOrEmpty(dt))
            {
                d = dt.StringtoDateTime();
            }
            return d.ToString("MM/dd/yyyy");
        }
        public static string DateTimetoStringShortDate(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
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
        //function to get cloud or test env
        public static bool GetCloudEnvironment(this HttpContext context,out string fullpath)
        {
            bool isCloud = false;
            var path = UriHelper.GetDisplayUrl(context.Request);
            fullpath = path;
           // var path = context.Request.Host.Host;
            if (path.ToLower().Contains("v2"))
            {
                isCloud = true;
            }
            return isCloud;
        }
        // function to get the full month name 
       public static string getMonthFullName(this DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day);

            return date.ToString("MMMM");
        }
        public static string ShowDatetimeView(this string date)
        {
            string dt = "";
            if(!string.IsNullOrEmpty(date))
            {
                DateTime dtt = Convert.ToDateTime(date);
                dt = dtt.ToString("MM/dd/yyyy hh:mm:ss");
            }
            else
            {
                dt = "--";
            }
            return dt;
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
