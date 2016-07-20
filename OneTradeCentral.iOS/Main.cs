using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin;
using Foundation;
using UIKit;


namespace OneTradeCentral.iOS
{
	public class Application


	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			
			//Insights.Initialize ("94cd7f50b4006093a473fbe19f0496b98eae0bc2");
			// if you want to use a different Application Delegate class firom "AppDelegate"
			// you can specify it here.
			try{
				
				//Insights.Track("MainMethod()", new Dictionary<string, string> {{"Main method.", "OnStart"}});
			UIApplication.Main (args, null, "AppDelegate");
			}
			catch(Exception ex){
				
			}

		
		}
	}
}
