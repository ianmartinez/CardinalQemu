using System;
using CardinalLib.Host;
using Eto.Drawing;

namespace CardinalQemu
{
    public enum IconSize
    {
        Small,
        Large
    }

    public enum IconResolution
    {
        Standard, Retina
    }

    public static class Icons
    {
        const string folder24 = "CardinalQemu.Resources.Icons24"; // 24px
        const string folder32 = "CardinalQemu.Resources.Icons32"; // 32px
        const string folder64 = "CardinalQemu.Resources.Icons64"; // 64px

        public static Icon Get(string name, IconSize size = IconSize.Small, IconResolution resolution = IconResolution.Retina)
        {
            string iconFolder;

            // The folder
            if (resolution == IconResolution.Standard)
                iconFolder = (size == IconSize.Small) ? folder24 : folder32;
            else
                iconFolder = (size == IconSize.Small) ? folder32 : folder64;

            // The size
            int sizePx = 0;
            switch(iconFolder)
            {
                case folder24:
                    sizePx = 24;
                    break;
                case folder32:
                    sizePx = 32;
                    break;
                case folder64:
                    sizePx = 64;
                    break;
            }

            if (resolution == IconResolution.Retina && HostSystem.IsMacOS)
                sizePx /= 2;

            var iconPath = string.Format("{0}.{1}.png", iconFolder, name);
            var iconResource = Icon.FromResource(iconPath);
            var icon = iconResource.WithSize(sizePx, sizePx);

            return icon;
        }
    }
}
