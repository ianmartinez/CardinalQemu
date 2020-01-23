using System.Linq;

namespace CardinalLib.Qemu
{
    public static class QemuData
    {
        public static string[] ArchNames =>
            (from app in QemuApps.Archs
             select app.Name.Replace("qemu-system-", "")).ToArray();
    }
}
