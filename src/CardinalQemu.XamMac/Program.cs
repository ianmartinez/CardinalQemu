using AppKit;
using Eto.Forms;

namespace CardinalQemu.XamMac
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			var platform = new Eto.Mac.Platform(); // mac platform

            // to register your custom control handler, call this before using your class:
            platform.Add<BlurView.IBlurView>(() => new BlurViewHandler());


			new Application(platform).Run(new MainForm());
		}
	}
}
