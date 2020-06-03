using System;
using System.Collections.Generic;

namespace CardinalLib.Qemu
{
    /// <summary>
    /// Handles responses delivered from some QEMU apps in the format of:
    ///    <para>key_1: value_1</para> 
    ///    <para>key_2: value_2</para> 
    ///    <para>key_n: value_n</para> 
    /// </summary>
    public class QemuDictionaryResponse
    {
        private readonly Dictionary<string, string> responses = new Dictionary<string, string>();

        /// <summary>
        /// Get a given key in the response, null if the key doesn't exist
        /// </summary>
        /// 
        /// <param name="key">The key to search for</param>
        /// 
        /// <returns>The value, if it exists, null if not</returns>
        public string this[string key] => (responses.ContainsKey(key)) ? responses[key] : null;

        /// <summary>
        /// Initialize a QemuDictionaryResponse with the ShellResult.Output array of lines
        /// </summary>
        /// 
        /// <param name="lines">An array of lines to parse</param>
        public QemuDictionaryResponse(string[] lines, string delimeter = ":")
        {
            foreach(var line in lines)
            {
                // Split key:value along the delimeter
                var kvSplit = line.Split(new[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);

                if(kvSplit.Length > 1) // Is a key value line
                {
                    var key = kvSplit[0].Trim();
                    var value = kvSplit[1].Trim();
                    responses[key] = value;
                }
            }
        }
    }
}