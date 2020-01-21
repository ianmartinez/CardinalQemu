using System;
using System.Collections.Generic;

namespace CardinalLib.Host
{
    public class ShellResult
    {
        /// <summary>
        /// The stderror output of a shell command
        /// </summary>
        public string[] Errors { get; internal set; }

        /// <summary>
        /// The stdout output of a shell command;
        /// </summary>
        public string[] Output { get; internal set; }
    }
}
