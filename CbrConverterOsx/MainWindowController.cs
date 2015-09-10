
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using CbrConverter;
using System.IO;

namespace cbrconverterosx
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{

		bool _fileSelected = false;
		//bool _outputFolderSelected = false;

		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		public override void WindowDidLoad ()
		{
			base.WindowDidLoad ();
			ProcessingLbl.StringValue = "";
			string version =	NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleShortVersionString").ToString ();

			this.Window.Title = "Cbr Converter " + version;
		}

		partial void SelectFile (NSObject sender)
		{
			var openPanel = new NSOpenPanel();
			openPanel.ReleasedWhenClosed = true;
			openPanel.Prompt = "Select a file or a folder";
			openPanel.CanChooseDirectories = true;

			var result = openPanel.RunModal();
			if (result == 1)
			{
				DataAccess.Instance.g_WorkingDir = openPanel.Url.ToString();//.Filename;
				if (DataAccess.Instance.g_WorkingDir.Contains("file://"))
					DataAccess.Instance.g_WorkingDir = DataAccess.Instance.g_WorkingDir.Replace("file://","");

				InputTbox.StringValue = DataAccess.Instance.g_WorkingDir;




			

				//check if file or folder
				if (File.Exists(DataAccess.Instance.g_WorkingDir)) //is a file
				{
					//check the extension
					var ext = Path.GetExtension(DataAccess.Instance.g_WorkingDir).ToLower();
					if (ext == ".pdf")
					{
						PdfCbzChk.State = NSCellStateValue.On;
						CbrPdfChk.State = NSCellStateValue.Off;
						JoinImgChk.State = NSCellStateValue.On;



					}
					else if (ext == ".cbr" || ext == ".cbz")
					{
						PdfCbzChk.State = NSCellStateValue.Off;
						CbrPdfChk.State = NSCellStateValue.On;
						JoinImgChk.State = NSCellStateValue.Off;

					}
				}
				else //is a folder
				{
					PdfCbzChk.State = NSCellStateValue.On;
					CbrPdfChk.State = NSCellStateValue.On;
					JoinImgChk.State = NSCellStateValue.On;

				}

				_fileSelected = true;

				//if (this.chk_SourceFolder.Checked) //always true for now
				{
				//	_outputFolderSelected = true;
					DataAccess.Instance.g_Output_dir = Path.GetDirectoryName(DataAccess.Instance.g_WorkingDir);
					//this.tbox_OuputFolder.Text = this.tbox_SourceFile.Text;
				}
			}
		}

		partial void BeginConversion (NSObject sender)
		{
			var pdf2cbz = PdfCbzChk.State == NSCellStateValue.On;
			var cbr2pdf = CbrPdfChk.State == NSCellStateValue.On;
			var reduce = ReduceSizeChk.State == NSCellStateValue.On;
			var joinimgs = JoinImgChk.State == NSCellStateValue.On;
			var delete = DeleteOrigChk.State == NSCellStateValue.On;

			CurrFileProgressBar.DoubleValue = 0;
			TotalProgressBar.DoubleValue = 0;

			if (DataAccess.Instance.g_Processing)
			{
				DataAccess.Instance.g_Processing = false;
				StartBtn.Title = "START";
				ProcessingLbl.StringValue = string.Empty;
			}
			else
			{
				if (((cbr2pdf) || (pdf2cbz))&&(_fileSelected))
				{
					StartBtn.Title = "STOP";
					DataAccess.Instance.g_ReduceSize = reduce;
					DataAccess.Instance.g_Processing = true;
					Extract ex = new Extract();
					this.Subscribe(ex);


					ex.BeginExtraction(cbr2pdf, pdf2cbz, reduce, delete,joinimgs, joinimgs, true);
				}
			}

			/*var openPanel = new NSOpenPanel();
			openPanel.ReleasedWhenClosed = true;
			openPanel.Prompt = "Select a file or a folder";
			openPanel.CanChooseDirectories = true;

			var result = openPanel.RunModal();
			if (result == 1)
			{
				

				DataAccess.Instance.g_Output_dir = "/Users/Xeo/Downloads/temp/t";
				//DataAccess.Instance.g_WorkingDir = "/Users/Xeo/Downloads/3120.cbr";
				DataAccess.Instance.g_Processing = true;

				Extract ex = new Extract();
				this.Subscribe(ex);


				ex.BeginExtraction(cbr2pdf, pdf2cbz, reduce, delete,joinimgs, joinimgs);


			}*/

		}

		public void Subscribe(Extract m)
		{
			m.evnt_UpdateCurBar += new Extract.UpdateCurrentBar(UpdateCurrBar);
			m.evnt_UpdatTotBar += new Extract.UpdateTotalBar(UpdateTotaBar);
			m.evnt_UpdateFileName += new Extract.UpdateFileName(UpdateFileName);
			m.evnt_ErrorNotify += new Extract.ErrorNotify(ErrorShowPopup);
			PdfFunctions.evnt_UpdateCurBar += new PdfFunctions.UpdateCurrentBar(UpdateCurrBar);
		}

		private void UpdateCurrBar()
		{           
			InvokeOnMainThread (() => {
				{
					if (DataAccess.Instance.g_curProgress > 100)
						DataAccess.Instance.g_curProgress = 100;
					CurrFileProgressBar.DoubleValue = DataAccess.Instance.g_curProgress;
				
				}});
		}

		private void UpdateFileName(Extract m, EventArgs e)
		{

			InvokeOnMainThread (() => {
				{
					ProcessingLbl.StringValue  = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
					if (ProcessingLbl.StringValue == string.Empty)//finished
					{
						StartBtn.Title = "START";


					}
				}});
		}

		private void UpdateTotaBar(Extract m, EventArgs e)
		{
			InvokeOnMainThread (() => {
				{
					if (DataAccess.Instance.g_totProgress > 100)
						DataAccess.Instance.g_totProgress = 100;
					 TotalProgressBar.DoubleValue = DataAccess.Instance.g_totProgress;
				}});
		}

		private void ErrorShowPopup(Extract m, string e)
		{
			/*InvokeOnMainThread (() => {
				{
					//btn_StartStop.Text = "START";
					//pbar_TotalProgress.Value = 0;
					//pbar_ActualFile.Value = 0;
					//lbl_ProcessingFile.Text =string.Empty;
					listViewLog.Items.Add(new ListViewItem(new List<string>{ (listViewLog.Items.Count + 1).ToString(), e}.ToArray()));
					//this.textBoxLog.Text += e + Environment.NewLine;
					ShowLog();
				}});
			//MessageBox.Show(e, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
*/
		}
	}
}

