using System;
using System.IO;
using System.Reflection;

namespace Sjg.IdentityCore
{
    /// <summary>
    /// Utilities to Get Project / Module Build Date
    /// </summary>
    public class BuildDate
    {
        /// <summary>
        /// Get Project Build Date created at Build Time from embedded static resource.
        /// </summary>
        /// <returns></returns>
        public static DateTime? Get()
        {
            // Sjg.IdentityCore.resources.BuildDate.txt - contains the date time - created in Pre-build event.
            // echo %date:~4,2%-%date:~7,2%-%date:~10,4% %time% > "$(ProjectDir)\resources\Sjg.IdentityCore\BuildDate.txt"
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.resources.Sjg.IdentityCore.BuildDate.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var dtStr = reader.ReadToEnd();

                    if (DateTime.TryParse(dtStr, out DateTime buildDate))
                    {
                        return buildDate;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get a date time version time stamp - Build Date in string format "yyyyMMddHHmmssff". Used in static resource versioning.
        /// </summary>
        /// <returns>Returns Build Date in format "yyyyMMddHHmmssff"</returns>
        public static string GetDateTimeVersion()
        {
            var buildDate = Get();
            return (buildDate.HasValue) ? buildDate.Value.ToString("yyyyMMddHHmmssff") : string.Empty;
        }
    }
}