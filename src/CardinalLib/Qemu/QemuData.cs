using System.Linq;

namespace CardinalLib.Qemu
{
    /// <summary>
    /// Get data about the user's QEMU installation to populate GUI
    /// </summary>
    public static class QemuData
    {
        /// <summary>
        /// Get a list of archs supported
        /// </summary>
        public static string[] ArchNames =>
            (from app in QemuApps.Archs
             select app.Name.Replace("qemu-system-", "")).ToArray();
    }
}
