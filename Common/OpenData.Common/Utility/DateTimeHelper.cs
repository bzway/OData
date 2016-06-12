using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace OpenData.Utility
{
    /// <summary>
    /// This structure is the equivalent of the Windows SYSTEMTIME structure.
    /// </summary>

    public static class DateTimeHelper
    {
        /// <summary>
        /// Parses from ticks OR datetime string.
        /// </summary>
        /// <param name="strValue">The STR value.</param>
        /// <returns></returns>
        public static DateTime Parse(string strValue)
        {
            DateTime dt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(strValue))
            {
                long ticks = 0;
                if (long.TryParse(strValue, out ticks))
                {
                    dt = new DateTime(ticks);
                }
                else
                {
                    DateTime.TryParse(strValue, out dt);
                }
            }

            return dt;
        }
        public static bool TryParse(string strValue, out DateTime dt)
        {
            dt = DateTime.Now;
            if (!string.IsNullOrEmpty(strValue))
            {
                long ticks = 0;
                if (long.TryParse(strValue, out ticks))
                {
                    dt = new DateTime(ticks);
                    return true;
                }
                else
                {
                    return DateTime.TryParse(strValue, out dt);
                }
            }
            return false;
        }

        public static bool BirthDayWeek(DateTime time)
        {
            DateTime dtBirthday = new DateTime(DateTime.Now.Year, time.Month, time.Day);
            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return (dtNow >= dtBirthday.AddDays(-3) && dtNow <= dtBirthday.AddDays(3));
        }



        static DateTime BaseTime = new DateTime(1970, 1, 1);//Unix起始时间

        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
        {
            return BaseTime.AddTicks((dateTimeFromXml + 8 * 60 * 60) * 10000000);
        }
        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
        {
            return GetDateTimeFromXml(long.Parse(dateTimeFromXml));
        }

        /// <summary>
        /// 获取微信DateTime
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long GetWeixinDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }
    }
}