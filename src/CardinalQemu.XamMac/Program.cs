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

			// to register your custom control handler, call this before using your class:
			//platform.Add<BlurView.IBlurView>(() => new BlurViewHandler());

			Eto.Style.Add<TreeGridViewHandler>("BlurView", handler => {
				var control = handler.Control;
				var widget = handler.Widget;

				var visualEffectView = new NSVisualEffectView();
				visualEffectView.TranslatesAutoresizingMaskIntoConstraints = false;
				visualEffectView.Material = NSVisualEffectMaterial.UnderWindowBackground;
				visualEffectView.State = NSVisualEffectState.FollowsWindowActiveState;
				var etoVisualEffectView = visualEffectView.ToEto();
				(widget.Parent as Panel).Content = etoVisualEffectView;
				visualEffectView.AddSubview(control);

			});

        /*    Eto.Style.Add<TreeGridViewHandler>("TransparentSelector", handler => {
				NSOutlineView control = handler.Control;
				var widget = handler.Widget;
				control.HeaderView = null;
				control.WantsLayer = false;
				control.GridColor = NSColor.Clear;
             foreach(var col in control.TableColumns())
                {
					col.TableView.BackgroundColor = NSColor.Clear;
					col.TableView.WantsLayer = false;
                }
				control.BackgroundColor = NSColor.Clear;
				//control.BackgroundColor = NSColor.Clear;

			});*/

			new Application(platform).Run(new MainForm());
		}
	}
}
