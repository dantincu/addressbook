using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class Utils
    {
        public const string ALLOWED_ORIGINS_APP_SETTINGS_KEY = "AllowedOrigins";

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

        public static T GetCfgValue<T>(this IConfiguration config, string[] sectionNames)
        {
            var section = config.GetSection(sectionNames[0]);

            for (int i = 1; i < sectionNames.Length; i++)
            {
                section = section.GetSection(sectionNames[i]);
            }

            var retVal = section.Get<T>();
            return retVal;
        }

        public static string[] GetAllowedOrigins(
            this IConfiguration config) => config.GetCfgValue<string[]>(
                [ ALLOWED_ORIGINS_APP_SETTINGS_KEY ]) ?? throw new InvalidOperationException(
                    string.Join(" ", "The appsettings file should contain an entry for key",
                        $"{ALLOWED_ORIGINS_APP_SETTINGS_KEY} but the current one doesn't"));
    }
}
