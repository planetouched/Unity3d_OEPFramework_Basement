using System;

namespace Basement.BLFramework.Essential.Time
{
    public static class TimeUtil
    {
        private static readonly DateTime _shift = new DateTime(1970, 1, 1);
        private static long _deltaTime;

        public static void SetDelta(long unixTimestamp)
        {
            _deltaTime = unixTimestamp - GetUnixTime();
        }

        public static long GetUnixTime(long unixTimestamp = 0)
        {
            return unixTimestamp == 0 ? (long)(DateTime.UtcNow - _shift).TotalSeconds + _deltaTime : unixTimestamp;
        }
    }
}
