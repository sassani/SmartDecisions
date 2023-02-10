using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public class DateTimeHelper
    {
        public static long GetUnixTimestamp(int minutesOffset = 0)
        {
            DateTime dt = DateTime.Now.AddMinutes(minutesOffset);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dt);
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public static long GetUnixTimestamp(DateTime dateTime)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}
