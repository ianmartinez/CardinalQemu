using System;
namespace CardinalQemu
{
    /// <summary>
    /// Get info about the app for about boxes and window titles
    /// </summary>
    public static class AppInfo
    {
        public static Version Version => new Version(0, 1);
        public static string FormattedVersion => Version.Major + "." + Version.Minor;
        public static int CopyrightYear => 2020;
    }
}
