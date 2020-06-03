using System.Collections.Generic;
using System.Linq;
using CardinalLib.Host;

namespace CardinalLib.Qemu
{
    /// <summary>
    /// A static class holding a list of
    /// </summary>
    public static class QemuApps
    {
        // If the app cache has been loaded
        private static bool initialized;

        // Store a cache of apps that is only loaded once at first use
        private static List<App> appCache = new List<App>();

        /// <summary>
        /// Reload the cache of apps
        /// </summary>
        public static void LoadApps()
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

        /// <summary>
        /// Get a QEMU app 
        /// </summary>
        /// 
        /// <param name="name">The app name</param>
        /// 
        /// <returns>A specific qemu-* app or null, if not found</returns>
        public static App Get(string name)
        {
            if (!initialized)
                LoadApps();

            return (from app in appCache
                    where app.Exists && app.Name == name
                    select app).FirstOrDefault();
        }

        /// <summary>
        /// All of the arch apps available on the system
        /// </summary>
        public static IEnumerable<App> Archs
        {
            get
            {
                if (!initialized)
                    LoadApps();

                return from app in appCache
                       where app.Exists && app.Name.StartsWith("qemu-system-")
                       select app;
            }
        }
    }
}
