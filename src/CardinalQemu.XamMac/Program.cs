using Eto.Forms;
using Eto.Mac.Forms;
using Eto.Mac.Forms.ToolBar;

namespace CardinalQemu.XamMac
{
    static class MainClass
	{
		static void Main(string[] args)
		{
			var platform = new Eto.Mac.Platform(); // mac platform

            // Allow user customization on macOS toolbars
			Eto.Style.Add<ToolBarHandler>("NativeToolbar", handler => {
				var toolbar = handler.Control;
				toolbar.AllowsUserCustomization = true;
				toolbar.AutosavesConfiguration = true;
			});

			// Allow app to become fullscreen
			Eto.Style.Add<ApplicationHandler>("Application", handler => {
				handler.EnableFullScreen();
			});

			new Application(platform).Run(new MainForm());
		}
	}
}
