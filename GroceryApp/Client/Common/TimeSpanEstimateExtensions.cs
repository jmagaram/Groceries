using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using static Models.ViewTypes;

#nullable enable
namespace GroceryApp.Common
{
    public static class TimeSpanEstimateExtensions
    {
        public static TimeSpan AsTimeSpan(this TimeSpanEstimate e) => TimeSpanEstimateModule.toTimeSpan(e);

        public static bool IsLessThanOrEqualTo(this TimeSpanEstimate e, TimeSpan t) => e.AsTimeSpan() <= t;

        public static bool IsZeroOrNegative(this TimeSpanEstimate e) => e.IsLessThanOrEqualTo(TimeSpan.Zero);

        public static string Format(
            this TimeSpanEstimate e,
            Func<int, string> days,
            Func<int, string> weeks,
            Func<int, string> months)
        {
            if (e is TimeSpanEstimate.Days d)
            {
                return days(d.Item);
            }
            else if (e is TimeSpanEstimate.Weeks w)
            {
                return weeks(w.Item);
            }
            else if (e is TimeSpanEstimate.Months m)
            {
                return months(m.Item);
            }
            else throw new NotImplementedException();
        }

        private static string JoinQuantityUnits(int n, string singular, string plural, string separator)
        {
            string units = ((n == 1) || (n == -1)) ? singular : plural;
            return $"{n}{separator}{units}";
        }

        public static string Format(
            this TimeSpanEstimate e,
            string separator,
            string days1,
            string daysN,
            string weeks1,
            string weeksN,
            string months1,
            string monthsN) => e.Format(i => JoinQuantityUnits(i, days1, daysN, separator),
                                        i => JoinQuantityUnits(i, weeks1, weeksN, separator),
                                        i => JoinQuantityUnits(i, months1, monthsN, separator));
    }
}
