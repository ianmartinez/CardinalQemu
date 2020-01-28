using AppKit;
using Eto.Forms;
using Eto.Mac.Forms.Controls;

namespace CardinalQemu.XamMac
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			var platform = new Eto.Mac.Platform(); // mac platform

			new Application(platform).Run(new MainForm());
		}
	}
}
