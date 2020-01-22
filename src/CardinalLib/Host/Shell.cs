using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CardinalLib.Host
{
    /// <summary>
    /// Handles interaction with the host's shell, be it bash/zshell on Unix
    /// or cmd.exe on Windows.Using this class provides a way of executing
    /// commands on the shell in a platform-independent manner.
    /// </summary>
    public static class Shell
    {
        /// <summary>
        /// The host shell executable name
        /// </summary>
        public static string Name => SystemInfo.IsUnix ? "sh" : "cmd.exe";

        /// <summary>
        /// The host shell's app to lookup other apps
        /// </summary>
        public static string LookupApp => SystemInfo.IsUnix ? "which" : "where";

        /// <summary>
        /// The command argument after the shell name for the host shell
        /// </summary>
        public static string CommandArg => SystemInfo.IsUnix ? "-c" : "/c";

        /// <summary>
        /// The line continuation character for the host shell
        /// </summary>
        public static string LineContinuation => (SystemInfo.IsUnix) ? " \\\n" : " ^\r\n";

        /// <summary>
        /// Make a string safe for the shell
        /// </summary>
        /// 
        /// <param name="rawString">The unsanitized string</param>
        /// 
        /// <returns>The string that has been sanitized</returns>
        private static string ShellSanitize(string rawString)
        {
            return rawString.Replace("\"", "\\\"");
        }

        /// <summary>
        /// Accepts a list of arguments and executes them on the system's shell,
        /// redirecting stdErr and stdOut to a ShellResult object
        /// </summary>
        /// 
        /// <param name="args">A param array of commands to execute</param>
        /// 
        /// <returns>A </returns>
        public static ShellResult Execute(ShellCommand command)
        {
            // Escape quotes for commands
            for(var i = 0; i< command.Arguments.Length; i++)
                command.Arguments[i] = ShellSanitize(command.Arguments[i]);

            // Change the working directory if it has been customized
            var cdCommand = (command.ChangeDirectory) ?
                string.Format(" cd {0} && ", ShellSanitize(command.WorkingDirectory)) : "";

            // Form arguments string + command arg, so on Unix, -c arg1 arg2 arg3 arg_n
            var argumentsString = string.Format("{0} \"{1}{2} {3}\"",
                                                CommandArg,
                                                cdCommand,
                                                command.Executable,
                                                string.Join(" ", command.Arguments));

            // Form process info
            var startInfo = new ProcessStartInfo
            {
                FileName = Name,
                Arguments = argumentsString,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            // Create process
            var process = new Process()
            {
               StartInfo = startInfo
            };

            // Run the process and redirect the outputs
            process.Start();
            string stdOut = process.StandardOutput.ReadToEnd();
            string stdError = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Ignore empty lines if specified in command object (default = true)
            var splitOptions = (command.IgnoreEmptyLines) ?
                StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;

            // Return the output
            return new ShellResult
            {
                Errors = stdError.Split(new[] { Environment.NewLine }, splitOptions),
                Output = stdOut.Split(new[] { Environment.NewLine }, splitOptions)
            };
        }
    }
}
