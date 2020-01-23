using System.IO;
using System.Threading.Tasks;

namespace CardinalLib.Host
{
    public class App
    {
        private static string[] UnixBinFolders = new string[] {
            "/usr/bin/", "/usr/local/bin"
        };

        private static string[] WinBinFolders = new string[] {
            @"C:\Program Files\qemu\"
        };

        public string Name { get; }
        public FileInfo Executable { get; set; }
        public bool Exists => Executable != null && Executable.Exists;
        

        public App(string appName)
        {
            Name = appName;
            var lookup = Shell.Execute(new ShellCommand(Shell.LookupApp, appName));
             
            if(lookup.IsValid) // If standard lookup via which/where succeeded
            {
                Executable = new FileInfo(lookup.Output[0]);
            }
            else // Manually lookup instead
            {
                var binFolders = HostSystem.IsUnix ? UnixBinFolders : WinBinFolders;
                var fileName = HostSystem.IsUnix ? appName : appName + ".exe";

                foreach (var folder in binFolders)
                {
                    var file = new FileInfo(Path.Combine(folder, fileName));

                    if (file.Exists)
                    {
                        Executable = file;
                        continue;
                    }
                }
            }
        }

        public ShellResult Run(string[] args = null)
        {
            if (Exists)
                return Shell.Execute(new ShellCommand(Executable.FullName, args ?? new string[0]));
            else
                return null;
        }

        public async Task<ShellResult> RunAsync(string[] args = null)
        {
            return await Task.Run(() => Run(args));
        }
    }
}
