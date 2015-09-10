// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace cbrconverterosx
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton CbrPdfChk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator CurrFileProgressBar { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton DeleteOrigChk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField InputTbox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton JoinImgChk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton PdfCbzChk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ProcessingLbl { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton ReduceSizeChk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton SelectFileBtn { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton StartBtn { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator TotalProgressBar { get; set; }

		[Action ("BeginConversion:")]
		partial void BeginConversion (MonoMac.Foundation.NSObject sender);

		[Action ("SelectFile:")]
		partial void SelectFile (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (InputTbox != null) {
				InputTbox.Dispose ();
				InputTbox = null;
			}

			if (StartBtn != null) {
				StartBtn.Dispose ();
				StartBtn = null;
			}

			if (PdfCbzChk != null) {
				PdfCbzChk.Dispose ();
				PdfCbzChk = null;
			}

			if (CbrPdfChk != null) {
				CbrPdfChk.Dispose ();
				CbrPdfChk = null;
			}

			if (DeleteOrigChk != null) {
				DeleteOrigChk.Dispose ();
				DeleteOrigChk = null;
			}

			if (JoinImgChk != null) {
				JoinImgChk.Dispose ();
				JoinImgChk = null;
			}

			if (ReduceSizeChk != null) {
				ReduceSizeChk.Dispose ();
				ReduceSizeChk = null;
			}

			if (CurrFileProgressBar != null) {
				CurrFileProgressBar.Dispose ();
				CurrFileProgressBar = null;
			}

			if (TotalProgressBar != null) {
				TotalProgressBar.Dispose ();
				TotalProgressBar = null;
			}

			if (ProcessingLbl != null) {
				ProcessingLbl.Dispose ();
				ProcessingLbl = null;
			}

			if (SelectFileBtn != null) {
				SelectFileBtn.Dispose ();
				SelectFileBtn = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
