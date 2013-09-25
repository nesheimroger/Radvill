using System;
using System.Collections.Generic;

namespace Radvill.Advisor.Private.Helpers
{
    public class TimeStampComparer : IComparer<DateTime?>
    {
        public int Compare(DateTime? x, DateTime? y)
        {
            if (!x.HasValue && y.HasValue)
            {
                return 1;
            }

            if (x.HasValue && !y.HasValue)
            {
                return -1;
            }

            if (!x.HasValue && !y.HasValue)
            {
                return 0;
            }

            if (x > y)
            {
                return -1;
            }

            if (x < y)
            {
                return 1;
            }

            return 0;
        }
    }
}