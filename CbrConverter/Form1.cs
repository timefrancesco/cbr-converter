using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CbrConverter
{
    public partial class Form1 : Form
    {
        bool _fileSelected = false;

        public Form1()
        {
            InitializeComponent();
        }

       

        private void btn_StartStop_Click(object sender, EventArgs e)
        {

            if (DataAccess.Instance.g_Processing)
            {
                DataAccess.Instance.g_Processing = false;
                btn_StartStop.Text = "START";
                lbl_ProcessingFile.Text = string.Empty;

            }
            else
            {
                if (((chk_cbr2pdf.Checked) || (chk_pdf2cbr.Checked))&&(_fileSelected))
                {
                    btn_StartStop.Text = "STOP";
                    DataAccess.Instance.g_ReduceSize = chk_ReduceSize.Checked;
                    DataAccess.Instance.g_Processing = true;
                    Extract ex = new Extract();
                    this.Subscribe(ex);
                    ex.BeginExtraction(chk_cbr2pdf.Checked, chk_pdf2cbr.Checked, chk_ReduceSize.Checked, chk_deleteOrig.Checked);
                }
            }
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
           
            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_curProgress > 100)
                    DataAccess.Instance.g_curProgress = 100;
                pbar_ActualFile.Value = (int)DataAccess.Instance.g_curProgress;
            });
        }

        private void UpdateFileName(Extract m, EventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                lbl_ProcessingFile.Text = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                if (lbl_ProcessingFile.Text == string.Empty)
                {
                    btn_StartStop.Text = "START";

                    //end remove original if selected
                    if (chk_deleteOrig.Checked)
                    {
                        if (File.Exists(DataAccess.Instance.g_WorkingDir)) //single dile
                            File.Delete(DataAccess.Instance.g_WorkingDir);
                        else
                            Directory.Delete(DataAccess.Instance.g_WorkingDir, true);
                    }
                }
            });
        }

        private void UpdateTotaBar(Extract m, EventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                if (DataAccess.Instance.g_totProgress > 100)
                    DataAccess.Instance.g_totProgress = 100;
                pbar_TotalProgress.Value = (int)DataAccess.Instance.g_totProgress;
            });
        }

        private void ErrorShowPopup(Extract m, string e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                btn_StartStop.Text = "START";
                pbar_TotalProgress.Value = 0;
                pbar_ActualFile.Value = 0;
                lbl_ProcessingFile.Text =string.Empty;
            });
            MessageBox.Show(e, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        /// <summary>
        /// User clicked on textbox, opendialog to select file or folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_SourceFile_Click(object sender, EventArgs e)
        {
            var SelectFolderDlg = new FolderBrowserDialogEx();
            SelectFolderDlg.Description = "Select a file or folder:"; //message
            SelectFolderDlg.ShowNewFolderButton = true;
            SelectFolderDlg.ShowEditBox = false;                     //editbox 
            SelectFolderDlg.ShowBothFilesAndFolders = true;          //show files and folders
            SelectFolderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer; //start from computer

            DialogResult result = SelectFolderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DataAccess.Instance.g_WorkingDir = SelectFolderDlg.SelectedPath;
                tbox_SourceFile.Text = SelectFolderDlg.SelectedPath;
                _fileSelected = true;
               
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbox_SourceFile.SelectionStart = 0;
        }

        private void lbl_about_Click(object sender, EventArgs e)
        {
            AboutorForm aboutform = new AboutorForm();
            aboutform.ShowDialog();
        }
    }
}
