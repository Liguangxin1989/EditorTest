using System;

namespace NetWrapper.Network.Tool
{
    /// <summary>
    /// Class TimeUtil 时间、日期实用工具类
    /// </summary>
    public static class TimeUtil
    {
        /// <summary>
        /// 将毫秒转换为TimeSpan类型
        /// </summary>
        /// <param name="ms">毫秒</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan MilSecondToTimeSpan(int ms)
        {
            return new TimeSpan(MilSecondToTicks(ms));
        }

        /// <summary>
        /// 将毫秒转换为长整形的Ticks
        /// </summary>
        /// <param name="ms"> 毫秒</param>
        /// <returns>Ticks</returns>
        public static long MilSecondToTicks(int ms)
        {
            return ms * 10000L;
        }

        /// <summary>
        /// Utc时间的秒数转换成DateTime类型
        /// </summary>
        /// <param name="utcSecond">Utc时间的秒数</param>
        /// <returns>转换后的时间</returns>
        public static DateTime UtcSecondToDateTime(int utcSecond)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(utcSecond);
        }

        /// <summary>
        /// DateTime转换成Utc时间的秒数
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>Utc时间的秒数</returns>
        public static int DateTimeToUtcSecond(DateTime dateTime)
        {
            TimeSpan utcTime = dateTime - new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32(utcTime.TotalSeconds);
        }
    }
}
