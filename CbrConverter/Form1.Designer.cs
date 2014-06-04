namespace CbrConverter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbox_SourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StartStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_about = new System.Windows.Forms.Label();
            this.chk_ReduceSize = new System.Windows.Forms.CheckBox();
            this.pbar_TotalProgress = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_ProcessingFile = new System.Windows.Forms.Label();
            this.pbar_ActualFile = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.chk_cbr2pdf = new System.Windows.Forms.CheckBox();
            this.chk_pdf2cbr = new System.Windows.Forms.CheckBox();
            this.chk_deleteOrig = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_about);
            this.groupBox1.Controls.Add(this.tbox_SourceFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tbox_SourceFile
            // 
            this.tbox_SourceFile.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tbox_SourceFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.tbox_SourceFile.Location = new System.Drawing.Point(6, 32);
            this.tbox_SourceFile.Name = "tbox_SourceFile";
            this.tbox_SourceFile.ReadOnly = true;
            this.tbox_SourceFile.Size = new System.Drawing.Size(298, 20);
            this.tbox_SourceFile.TabIndex = 5;
            this.tbox_SourceFile.Text = "Click here to select a file or folder";
            this.tbox_SourceFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbox_SourceFile.Click += new System.EventHandler(this.tbox_SourceFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select File or Folder";
            // 
            // btn_StartStop
            // 
            this.btn_StartStop.Location = new System.Drawing.Point(5, 17);
            this.btn_StartStop.Name = "btn_StartStop";
            this.btn_StartStop.Size = new System.Drawing.Size(67, 115);
            this.btn_StartStop.TabIndex = 2;
            this.btn_StartStop.Text = "START";
            this.btn_StartStop.UseVisualStyleBackColor = true;
            this.btn_StartStop.Click += new System.EventHandler(this.btn_StartStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk_deleteOrig);
            this.groupBox2.Controls.Add(this.chk_pdf2cbr);
            this.groupBox2.Controls.Add(this.chk_cbr2pdf);
            this.groupBox2.Controls.Add(this.chk_ReduceSize);
            this.groupBox2.Controls.Add(this.pbar_TotalProgress);
            this.groupBox2.Controls.Add(this.btn_StartStop);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lbl_ProcessingFile);
            this.groupBox2.Controls.Add(this.pbar_ActualFile);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(11, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 139);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // lbl_about
            // 
            this.lbl_about.AutoSize = true;
            this.lbl_about.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_about.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_about.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_about.Location = new System.Drawing.Point(269, 11);
            this.lbl_about.Name = "lbl_about";
            this.lbl_about.Size = new System.Drawing.Size(35, 13);
            this.lbl_about.TabIndex = 5;
            this.lbl_about.Text = "About";
            this.lbl_about.Click += new System.EventHandler(this.lbl_about_Click);
            // 
            // chk_ReduceSize
            // 
            this.chk_ReduceSize.AutoSize = true;
            this.chk_ReduceSize.Location = new System.Drawing.Point(197, 17);
            this.chk_ReduceSize.Name = "chk_ReduceSize";
            this.chk_ReduceSize.Size = new System.Drawing.Size(106, 17);
            this.chk_ReduceSize.TabIndex = 5;
            this.chk_ReduceSize.Text = "Reduce File Size";
            this.chk_ReduceSize.UseVisualStyleBackColor = true;
            // 
            // pbar_TotalProgress
            // 
            this.pbar_TotalProgress.Location = new System.Drawing.Point(78, 117);
            this.pbar_TotalProgress.Name = "pbar_TotalProgress";
            this.pbar_TotalProgress.Size = new System.Drawing.Size(225, 15);
            this.pbar_TotalProgress.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Total:";
            // 
            // lbl_ProcessingFile
            // 
            this.lbl_ProcessingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ProcessingFile.Location = new System.Drawing.Point(143, 67);
            this.lbl_ProcessingFile.Name = "lbl_ProcessingFile";
            this.lbl_ProcessingFile.Size = new System.Drawing.Size(160, 13);
            this.lbl_ProcessingFile.TabIndex = 2;
            // 
            // pbar_ActualFile
            // 
            this.pbar_ActualFile.Location = new System.Drawing.Point(78, 83);
            this.pbar_ActualFile.Name = "pbar_ActualFile";
            this.pbar_ActualFile.Size = new System.Drawing.Size(225, 15);
            this.pbar_ActualFile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Processing:";
            // 
            // chk_cbr2pdf
            // 
            this.chk_cbr2pdf.AutoSize = true;
            this.chk_cbr2pdf.Checked = true;
            this.chk_cbr2pdf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_cbr2pdf.Location = new System.Drawing.Point(78, 17);
            this.chk_cbr2pdf.Name = "chk_cbr2pdf";
            this.chk_cbr2pdf.Size = new System.Drawing.Size(101, 17);
            this.chk_cbr2pdf.TabIndex = 6;
            this.chk_cbr2pdf.Text = "Cbr/Cbz to PDF";
            this.chk_cbr2pdf.UseVisualStyleBackColor = true;
            // 
            // chk_pdf2cbr
            // 
            this.chk_pdf2cbr.AutoSize = true;
            this.chk_pdf2cbr.Checked = true;
            this.chk_pdf2cbr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_pdf2cbr.Location = new System.Drawing.Point(78, 38);
            this.chk_pdf2cbr.Name = "chk_pdf2cbr";
            this.chk_pdf2cbr.Size = new System.Drawing.Size(80, 17);
            this.chk_pdf2cbr.TabIndex = 7;
            this.chk_pdf2cbr.Text = "PDF to Cbz";
            this.chk_pdf2cbr.UseVisualStyleBackColor = true;
            // 
            // chk_deleteOrig
            // 
            this.chk_deleteOrig.AutoSize = true;
            this.chk_deleteOrig.Location = new System.Drawing.Point(197, 38);
            this.chk_deleteOrig.Name = "chk_deleteOrig";
            this.chk_deleteOrig.Size = new System.Drawing.Size(93, 17);
            this.chk_deleteOrig.TabIndex = 8;
            this.chk_deleteOrig.Text = "Delete original";
            this.chk_deleteOrig.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 228);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CBR Converter 1.1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbox_SourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_StartStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar pbar_TotalProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_ProcessingFile;
        private System.Windows.Forms.ProgressBar pbar_ActualFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_ReduceSize;
        private System.Windows.Forms.Label lbl_about;
        private System.Windows.Forms.CheckBox chk_deleteOrig;
        private System.Windows.Forms.CheckBox chk_pdf2cbr;
        private System.Windows.Forms.CheckBox chk_cbr2pdf;
    }
}

