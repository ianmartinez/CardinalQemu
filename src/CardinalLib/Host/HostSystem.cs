using System.Runtime.InteropServices;

namespace CardinalLib.Host
{
    public static class HostSystem
    {
        /// <summary>
        /// If the host OS is Windows
        /// </summary>
        public static bool IsWindows =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// If the host OS is macOS/OS X
        /// </summary>
        public static bool IsMacOS =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// If the host OS is Linux
        /// </summary>
        public static bool IsLinux =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// If the host OS is Unix
        /// </summary>
        public static bool IsUnix => !IsWindows;

        /// <summary>
        /// Get the platform of the host
        /// </summary>
        public static OSPlatform Platform
        {
            get
            {
                if (IsMacOS)
                    return OSPlatform.OSX;
                else if (IsWindows)
                    return OSPlatform.Windows;
                else
                    return OSPlatform.Linux;
            }
        }
    }
}
