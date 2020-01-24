using System;
namespace CardinalQemu
{
    public static class AppInfo
    {
        public static Version Version => new Version(0, 1);
        public static string FormattedVersion => Version.Major + "." + Version.Minor;
        public static int CopyrightYear => 2020;
    }
}
