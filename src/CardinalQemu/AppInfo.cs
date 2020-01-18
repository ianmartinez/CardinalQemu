using System;
namespace CardinalQemu
{
    public static class AppInfo
    {
        public static Version Version => new Version(0, 1);

        public static string GetFormattedVersion()
        {
            return Version.Major + "." + Version.Minor;
        }
    }
}
