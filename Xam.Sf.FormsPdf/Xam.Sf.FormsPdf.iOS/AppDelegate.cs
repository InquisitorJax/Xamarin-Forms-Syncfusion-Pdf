﻿
using Foundation;
using Samples.Syncfusion.XamarinForms.Pdf;
using UIKit;
using Wibci.Xamarin.Images;
using Xamarin.Forms;

namespace Xam.Sf.FormsPdf.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			Syncfusion.SfPdfViewer.XForms.iOS.SfPdfDocumentViewRenderer.Init();
			Syncfusion.SfRangeSlider.XForms.iOS.SfRangeSliderRenderer.Init();
			Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();

			LoadApplication(new App());

			DependencyService.Register<ISaveFileStreamCommand, iOSSaveFileStreamCommand>();
			DependencyService.Register<IResizeImageCommand, Wibci.Xamarin.Images.iOS.iOSImageResizeCommand>();
			DependencyService.Register<IAnalyseImageCommand, Wibci.Xamarin.Images.iOS.iOSAnalyseImageCommand>();
			DependencyService.Register<IImageUtility, AppleImageUtility>();


			return base.FinishedLaunching(app, options);
		}
	}
}
