using System;
using AppKit;
using Eto.Mac.Forms;
using Eto.Mac.Forms.Controls;

namespace CardinalQemu.XamMac
{
    public class BlurViewHandler : MacPanel<NSVisualEffectView, BlurView, BlurView.ICallback>, BlurView.IBlurView
	{
		public BlurViewHandler()
		{
			Control = new NSVisualEffectView();
		}
		public override NSView ContainerControl => Control;
	}
}
