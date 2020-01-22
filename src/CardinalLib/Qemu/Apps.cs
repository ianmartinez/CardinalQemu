using System.Collections.Generic;
using System.Linq;
using CardinalLib.Host;

namespace CardinalLib.Qemu
{
    public static class QemuApp
    {
        private static bool initialized;
        private static List<App> appCache = new List<App>();

        public static void ReloadApps()
        {
            appCache.Clear();
            appCache.AddRange(new App[] {
                new App("qemu-edid"),
                new App("qemu-img"),
                new App("qemu-io"),
                new App("qemu-nbd"),
                new App("qemu-system-aarch64"),
                new App("qemu-system-alpha"),
                new App("qemu-system-arm"),
                new App("qemu-system-cris"),
                new App("qemu-system-hppa"),
                new App("qemu-system-i386"),
                new App("qemu-system-lm32"),
                new App("qemu-system-m68k"),
                new App("qemu-system-microblaze"),
                new App("qemu-system-microblazeel"),
                new App("qemu-system-mips"),
                new App("qemu-system-mips64"),
                new App("qemu-system-mips64el"),
                new App("qemu-system-mipsel"),
                new App("qemu-system-moxie"),
                new App("qemu-system-nios2"),
                new App("qemu-system-or1k"),
                new App("qemu-system-ppc"),
                new App("qemu-system-ppc64"),
                new App("qemu-system-riscv32"),
                new App("qemu-system-riscv64"),
                new App("qemu-system-s390x"),
                new App("qemu-system-sh4"),
                new App("qemu-system-sh4eb"),
                new App("qemu-system-sparc"),
                new App("qemu-system-sparc64"),
                new App("qemu-system-tricore"),
                new App("qemu-system-unicore32"),
                new App("qemu-system-x86_64"),
                new App("qemu-system-xtensa"),
                new App("qemu-system-xtensaeb")
            });

            initialized = true;
        }

        public static App Get(string name)
        {
            if (!initialized)
                ReloadApps();

            return (from app in appCache
                    where app.Name == name
                    select app).FirstOrDefault();
        }
    }
}
