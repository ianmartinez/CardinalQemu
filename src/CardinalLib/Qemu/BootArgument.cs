using System.Collections.Generic;

namespace CardinalLib.Qemu
{
    public class BootArgument
    {
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public bool HasQuotes { get; set; } = false;
        public bool HasDash { get; set; } = true;

        public BootArgument(string name, string value)
        {
            Name = name;
            Values.Add(value);
        }

        public BootArgument(string name, IEnumerable<string> values)
        {
            Name = name;
            Values.AddRange(values);
        }
    }
}