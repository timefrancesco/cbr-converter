using System;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace cbrconverterosx
{
	public class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;
		
		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);


		}

		/*public override void DidFinishLaunching (NSNotification notification)
		{
			var menu = new NSMenu ();

			var menuItem = new NSMenuItem ();
			menu.AddItem (menuItem);

			var appMenu = new NSMenu ();
			var quitItem = new NSMenuItem ("Quit " + NSProcessInfo.ProcessInfo.ProcessName, "q", (s, e) => NSApplication.SharedApplication.Terminate (menu));
			appMenu.AddItem (quitItem);

			menuItem.Submenu = appMenu;
			NSApplication.SharedApplication.MainMenu = menu;

			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);
		}*/
	}
}

