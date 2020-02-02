using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.XPath;
using CardinalLib.Core;
using CardinalLib.Guest;
using CardinalLib.Hardware;
using CardinalLib.Host;
using CardinalLib.Qemu;

namespace CardinalLib.Machines
{
    public class Machine
    {
        public const bool FORCE_SDL_ON_WINDOWS = true;

        #region "Properties"
        public string MachineDirectory { get; private set; }
        public App SystemApp { get; private set; }
        public bool CanBoot => SystemApp != null;

        // Info
        public string Name { get; set; }
        public string Notes { get; set; }
        public OsInfo Os { get; set; }
        public bool IsRunning { get; private set; }

        // System
        private string arch;
        public string Arch {
            get => arch;

            set
            {
                arch = value;
                SystemApp = QemuApps.Get("qemu-system-" + arch);
            }
        }
        public string Cpu { get; set; }
        public string EmulatedMachine { get; set; }

        // Hardware
        public ByteValue Ram { get; set; }
        public List<string> CdDrives { get; set; } = new List<string>();
        public List<Disk> Disks { get; set; } = new List<Disk>();

        // Boot
        public string Kernel { get; set; }
        public string BootTarget { get; set; }

        // QEMU
        public List<ShellArgument> QemuArgs { get; set; } = new List<ShellArgument>();
        #endregion

        public Machine() { }

        public Machine(string machineFile)
        {
            // Get the directory that the file is in
            MachineDirectory = Path.GetDirectoryName(machineFile);

            // Read the machine.xml file
            var docNav = new XPathDocument(machineFile);
            var nav = docNav.CreateNavigator();

            // Notes
            Name = nav.SelectSingleNode("/machine/info/name")?.Value;
            Notes = nav.SelectSingleNode("/machine/info/notes")?.Value;

            // Guest Os Info
            Os = new OsInfo
            {
                Family = nav.SelectSingleNode("/machine/os/family").Value,
                Name = nav.SelectSingleNode("/machine/os/name").Value,
                Version = nav.SelectSingleNode("/machine/os/version").Value
            };

            // System
            Arch = nav.SelectSingleNode("/machine/setup/arch").Value;
            Cpu = nav.SelectSingleNode("/machine/setup/cpu")?.Value;
            EmulatedMachine = nav.SelectSingleNode("/machine/setup/machine")?.Value;
            BootTarget = nav.SelectSingleNode("/machine/setup/boot")?.Value;

            // Hardware
            Ram = new ByteValue(nav.SelectSingleNode("/machine/hardware/ram").ValueAsLong, ByteFormat.MB);

            foreach(XPathNavigator hardware in nav.SelectSingleNode("/machine/hardware").Select("*"))
            {
                var absoluteValue = ReplaceVariables(hardware.Value);
                switch(hardware.Name)
                {
                    case "cd":
                        CdDrives.Add(absoluteValue);
                        break;
                    case "disk":
                        if (!File.Exists(absoluteValue))
                            absoluteValue = Path.Combine(Directories.Disks, absoluteValue);
                        Disks.Add(new Disk(absoluteValue, hardware.GetAttribute("name", "")));
                        break;
                }
            }

            // QEMU arguments
            foreach (XPathNavigator bootArgument in nav.SelectSingleNode("/machine/qemu-args").Select("*"))
            {
                var argumentName = bootArgument.GetAttribute("name", "");
                var argumentValue = ReplaceVariables(bootArgument.Value);
                QemuArgs.Add(new ShellArgument(argumentName, argumentValue));
            }
        }

        public async Task<ShellResult> Boot()
        {
            List<ShellArgument> bootArgs = new List<ShellArgument>();

            // RAM
            bootArgs.Add(new ShellArgument("m", Ram.ToString()));


            // CPU
            if (!string.IsNullOrEmpty(Cpu))
            {
                bootArgs.Add(new ShellArgument("cpu", Cpu)
                {
                    HasQuotes = true
                });
            }

            // Emulated Machine
            if(!string.IsNullOrEmpty(EmulatedMachine))
            {
                bootArgs.Add(new ShellArgument("M", EmulatedMachine)
                {
                    HasQuotes = true
                });
            }

            // Disks
            foreach (var disk in Disks)
            {
                bootArgs.Add(new ShellArgument(disk.Drive, disk.AbsolutePath)
                {
                    HasQuotes = true
                });
            }

            // CDs
            foreach(var cd in CdDrives)
            {
                bootArgs.Add(new ShellArgument("cdrom", cd)
                {
                    HasQuotes = true
                });
            }

            // Boot target
            bootArgs.Add(new ShellArgument("boot", BootTarget));

            // Kernel
            if (!string.IsNullOrEmpty(Kernel))
            {
                bootArgs.Add(new ShellArgument("kernel", Kernel)
                {
                    HasQuotes = true
                });
            }

            /* Force SDL on Windows, the default GTK display 
               is too buggy and cursors jump out of guest randomly */
            if(HostSystem.IsWindows && FORCE_SDL_ON_WINDOWS)
            {
                bootArgs.Add(new ShellArgument("display", "sdl"));
            }

            // Additional QEMU arguments
            bootArgs.AddRange(QemuArgs);

            // Run boot the machine
            return await SystemApp.RunAsync(ShellArgument.ToArray(bootArgs.ToArray()));
        }

        #region "Variables"
        private string ReplaceVariables(string text)
        {
            string newText = (string)text.Clone();

            newText = Variable.Replace(newText, "HOME", Directories.UserHome, true);
            newText = Variable.Replace(newText, "MACHINES", Directories.Machines, true);
            newText = Variable.Replace(newText, "CURRENT", MachineDirectory, true);
            newText = Variable.Replace(newText, "DISKS", Directories.Disks, true);

            return newText;
        }

        private string ReverseVariables(string text)
        {
            string newText = (string)text.Clone();

            newText = Variable.Reverse(newText, "CURRENT", MachineDirectory);
            newText = Variable.Reverse(newText, "DISKS", Directories.Disks);
            newText = Variable.Reverse(newText, "MACHINES", Directories.Machines);
            newText = Variable.Reverse(newText, "HOME", Directories.UserHome);

            return newText;
        }
        #endregion

        #region "Static"
        public static Machine[] GetAll()
        {
            List<Machine> machines = new List<Machine>();

            // Create a new machine for each file
            foreach (var machineFolder in Directory.EnumerateDirectories(Directories.Machines))
            {
                var file = Path.Combine(machineFolder, "machine.xml");
                var fileInfo = new FileInfo(file);

                if (fileInfo.Exists)
                {
                    var newMachine = new Machine(file);
                    machines.Add(newMachine);
                }
            }

            // Sort alphabetically
            machines.Sort((machineA, machineB) => machineA.Name.CompareTo(machineB.Name));

            return machines.ToArray();
        }
        #endregion
    }
}
