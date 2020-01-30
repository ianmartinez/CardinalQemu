using AppKit;
using Eto.Forms;
using Eto.Mac.Forms.Controls;
using Eto.Mac.Forms.ToolBar;

namespace CardinalQemu.XamMac
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			var platform = new Eto.Mac.Platform(); // mac platform

            // Allow user customization on macOS toolbars
			Eto.Style.Add<ToolBarHandler>("nativeToolbar", handler => {
				var toolbar = handler.Control;
				toolbar.AllowsUserCustomization = true;
				toolbar.AutosavesConfiguration = true;
			});


            // TODO: add touchbar items

			new Application(platform).Run(new MainForm());
		}
	}
}
