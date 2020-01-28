using System;
using Eto.Drawing;

namespace CardinalQemu
{

    public enum Resolution
    {
        Standard, Retina
    }

    public static class Icons
    {
        const string standardFolder = "CardinalQemu.Resources.Icons";
        const string retinaFolder = "CardinalQemu.Resources.Icons";

        public static Icon Get(string name, Resolution resolution = Resolution.Retina)
        {
            var iconFolder = (resolution == Resolution.Retina) ? retinaFolder : standardFolder;
            return Icon.FromResource(String.Format("{0}.{1}.png", iconFolder, name));
        }
    }
}
