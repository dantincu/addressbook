using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class Utils
    {
        public static string ToUserFriendlyEnumeration<T>(
            this IEnumerable<T> nmrbl,
            string? firstItemPfxStr,
            string? defaultItemPfxStr,
            string? lastItemPfxStr,
            Func<T, int, string?>? stringConverter = null)
        {
            stringConverter ??= (val, idx) => val?.ToString();

            var strArr = nmrbl.Select(
                stringConverter).ToArray();

            int maxIdx = strArr.Length - 1;

            var retStr = string.Concat(
                strArr.Select(
                    (str, idx) =>
                    {
                        string? pfx;

                        if (idx == 0)
                        {
                            pfx = firstItemPfxStr;
                        }
                        else if (idx < maxIdx)
                        {
                            pfx = defaultItemPfxStr;
                        }
                        else
                        {
                            pfx = lastItemPfxStr;
                        }

                        var retItemStr = pfx + str;
                        return retItemStr;
                    }));

            return retStr;
        }
    }
}
