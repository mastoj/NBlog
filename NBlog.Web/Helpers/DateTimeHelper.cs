﻿using System;

namespace NBlog.Web.Helpers
{
    public static class DateTimeHelper
    {
        public static string ToHtmlTimeString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-ddThh:mm");
        }

        public static string ToPostTime(this DateTime datetime)
        {
            var endNumber = datetime.Day%10;
            string suffix;
            switch (endNumber)
            {
                case 1:
                    {
                        suffix = "st";
                        break;
                    }
                case 2:
                    {
                        suffix = "nd";
                        break;
                    }
                case 3:
                    {
                        suffix = "rd";
                        break;
                    }
                default:
                    {
                        suffix = "th";
                        break;
                    }
            }
            var datetimeString = datetime.ToString("MMM") + " " + datetime.Day + suffix + ", " + datetime.Year;
            return datetimeString;
        }
    }
}