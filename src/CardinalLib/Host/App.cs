using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CardinalLib.Host
{
    public class App
    {
        public string Name { get; }
        public FileInfo Executable { get; set; }
        public bool Exists => Executable != null && Executable.Exists;

        public App(string appName)
        {
            Name = appName;
            var lookup = Shell.Execute(new ShellCommand(Shell.LookupApp, appName));

            if(!lookup.HasErrors && lookup.HasOutput)
                Executable = new FileInfo(lookup.Output[0]);
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
