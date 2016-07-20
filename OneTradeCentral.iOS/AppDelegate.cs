using System;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launcOptions)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				var splitViewController = (UISplitViewController)Window.RootViewController;
				splitViewController.Delegate = new MainViewController.SplitViewDelegate ();
			}
			return true;
		}

		public override void OnActivated (UIApplication application)
		{
//			Logger.log ("OnActivated invoked.");
			BackgroundWorker.Instance.UploadAllPending ();

		}

		// This method is called as part of the transiton from background to active state.
//		public override void WillEnterForeground (UIApplication application)
//		{
//			Logger.log ("WillEnterForeground invoked.");
//		}

		// This method is invoked when the application is about to move from active to inactive state.
		//
		// OpenGL applications should use this method to pause.
		//
//		public override void OnResignActivation (UIApplication application)
//		{
//			Logger.log ("OnResignActivation invoked.");
//		}

		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background execution this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
//			Logger.log ("DidEnterBackground invoked.");
			BackgroundWorker.Instance.UploadAllPending ();
		}

		// This method is called when the application is about to terminate. Save data, if needed. 
//		public override void WillTerminate (UIApplication application)
//		{
//			Logger.log ("WillTerminate invoked.");
//		}
	}
}

