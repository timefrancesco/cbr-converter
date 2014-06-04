using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Drawing.Imaging;
using System.Drawing;
using SharpCompress;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Writer;
using SharpCompress.Archive.Zip;
using Ionic.Zip;

namespace CbrConverter
{
    public class Extract
    {
        string temporaryDir;
        public event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();
        public event UpdateTotalBar evnt_UpdatTotBar;
        public delegate void UpdateTotalBar(Extract m, EventArgs e);
        public event UpdateFileName evnt_UpdateFileName;
        public delegate void UpdateFileName(Extract m, EventArgs e);
        public event ErrorNotify evnt_ErrorNotify;
        public delegate void ErrorNotify(Extract m, string e);
        public EventArgs e = null;
        double CurOneStep;
        bool IsSingleFile; //to link the 2 progress bar
        bool _Cbr2Pdf, _Pdf2Cbz, _ReduceSize, _DeleteOriginal;


        /// <summary>
        /// Start the extraction thread according to selected file or folder
        /// </summary>
        public void BeginExtraction(bool cbr2pdf, bool pdf2cbz, bool reduceSize, bool deleteOriginal)
        {
            _Cbr2Pdf = cbr2pdf;
            _DeleteOriginal = deleteOriginal;
            _ReduceSize = reduceSize;
            _Pdf2Cbz = pdf2cbz;

            //checking if is a directory or a file
            if (File.Exists(DataAccess.Instance.g_WorkingDir))
            {
                DataAccess.Instance.g_WorkingFile = DataAccess.Instance.g_WorkingDir;
                IsSingleFile = true; 
                //it's a file so i start the single extraction thread
                //check if cbr
                string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile);
                if (((string.Compare(ext, ".cbr") == 0) || (string.Compare(ext, ".cbz") == 0)|| (string.Compare(ext, ".CBZ") == 0)|| (string.Compare(ext, ".CBR") == 0))&&(_Cbr2Pdf))
                {

                    Thread extract = new Thread(ExtractSingleFile);
                    extract.Start();
                }
                else if (((string.Compare(ext, ".pdf") == 0) || (string.Compare(ext, ".PDF") == 0))&&(_Pdf2Cbz))
                {
                    Thread compress = new Thread(CompressSingleFile);
                    compress.Start();
                }

            }
            else //it's a directory
            {
                IsSingleFile = false; 
                Thread ext = new Thread(ExtractMultipleFiles);
                ext.Start();
            }
        }

        /// <summary>
        /// Extract the file and call the Compress Image and the Generate PDF
        /// </summary>
        private void ExtractSingleFile()
        {
            try
            {
                //check if cbr
                /*string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile);
                if ((string.Compare(ext, ".cbr") != 0)&&(string.Compare(ext, ".cbz") != 0))
                    return;*/

                //write the name of the file on the UI
                evnt_UpdateFileName(this, e);



                //creating directory for extraction      
                string tempFileName = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
                temporaryDir = DataAccess.Instance.g_WorkingFile;
                temporaryDir = temporaryDir.Replace(tempFileName, Path.GetFileNameWithoutExtension(DataAccess.Instance.g_WorkingFile));
                //if the directory already exist, delete it
                if (Directory.Exists(temporaryDir))
                    Directory.Delete(temporaryDir, true);
                Directory.CreateDirectory(temporaryDir);           


                //inizio test
                var archive = ArchiveFactory.Open(DataAccess.Instance.g_WorkingFile);
                //calculating file for pregress bar
                double CurOneStep = archive.Entries.Count();
                int divider;
                if (_ReduceSize)
                    divider = 33;
                else
                    divider = 50;
                CurOneStep = divider / CurOneStep;

                //extract the file into the folder
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        if (DataAccess.Instance.g_Processing) //this is to stop the thread if stop button is pressed
                        {

                            string path = Path.Combine(temporaryDir, Path.GetFileName(entry.FilePath));
                            //entry.WriteToDirectory(@"C:\temp", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            entry.WriteToFile(path, ExtractOptions.Overwrite);

                            DataAccess.Instance.g_curProgress += CurOneStep;
                            evnt_UpdateCurBar();

                            if (IsSingleFile)
                            {
                                DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                                evnt_UpdatTotBar(this, e);
                            }
                        }
                    }
                }
                

                if (DataAccess.Instance.g_Processing)
                {
                    if (DataAccess.Instance.g_ReduceSize)
                        CompressImage();
                    GeneratePdf();
                }
                //forcing garbage collector to clean to avoid lock file error caused by pdfsharp
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //deleting temp dir
                Directory.Delete(temporaryDir, true);

                //update progress bar
                DataAccess.Instance.g_curProgress = 0;
                evnt_UpdateCurBar();

                if (IsSingleFile)
                {
                    DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                    evnt_UpdatTotBar(this, e);
                }

                //if we are converting a single file and not a directory we are done, so i reset values and clean the UI
                if (IsSingleFile)
                {
                    DataAccess.Instance.g_Processing = false;
                    DataAccess.Instance.g_WorkingFile = string.Empty;
                    evnt_UpdateFileName(this, e);
                }
            }
            catch (Exception ex) //too lazy to catch specific exceptions, TODO in future!!
            {
                DataAccess.Instance.g_Processing = false; //stopping the process
                evnt_ErrorNotify(this, ex.ToString());
            }
        }

        /// <summary>
        /// Extract images from PDF and compress into a cbr archive
        /// </summary>
        private void CompressSingleFile()
        {
            evnt_UpdateFileName(this, e);
            //creating directory for extraction      
            string tempFileName = Path.GetFileName(DataAccess.Instance.g_WorkingFile);
            string temporaryDir = DataAccess.Instance.g_WorkingFile;
            temporaryDir = temporaryDir.Replace(tempFileName, Path.GetFileNameWithoutExtension(DataAccess.Instance.g_WorkingFile));
            
            //if the directory already exist, delete it
            if (Directory.Exists(temporaryDir))
                Directory.Delete(temporaryDir, true);
            Directory.CreateDirectory(temporaryDir);

            
            int divider;
            if (_ReduceSize)
                divider = 50;
            else
                divider = 80;
           

            if (PdfFunctions.PDF_ExportImage(DataAccess.Instance.g_WorkingFile, temporaryDir, divider))
            {
                if (DataAccess.Instance.g_Processing)
                {
                    //compress files in a zip  
                    if (_ReduceSize)
                        CompressImage();

                    string savedfile = temporaryDir + ".cbz";


                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(temporaryDir);
                       // zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                        zip.Save(savedfile);
                    }

                    //waiting the new sharpcompress release to fix it
                  /*  using (var archive = ZipArchive.Create())
                    {
                        archive.AddAllFromDirectory(temporaryDir);                       
                        archive.SaveTo(savedfile, CompressionType.None);    
                    }*/

                    
                    //update progress bar
                    DataAccess.Instance.g_curProgress = 0;
                    evnt_UpdateCurBar();
                   
                }
                    
            }
            //delete the temp dir
            if (Directory.Exists(temporaryDir))
                Directory.Delete(temporaryDir, true);

            //if we are converting a single file and not a directory we are done, so i reset values and clean the UI
            if (IsSingleFile)
            {
                DataAccess.Instance.g_Processing = false;
                DataAccess.Instance.g_WorkingFile = string.Empty;
                evnt_UpdateFileName(this, e);
            }

            

        }

        
        /// <summary>
        /// when a directory is selected this method  is launched, it just read all the files inside the directory and call ExtractSingleFile()
        /// </summary>
        private void ExtractMultipleFiles()
        {
            string[] files = Directory.GetFiles(DataAccess.Instance.g_WorkingDir);
            int count = files.Count();

            //calculate the value for the progression bar
            double singval =(double) 100 / count;

            //call the ExtractSingleFile for each file
            foreach (string file in files)
            {
                if (DataAccess.Instance.g_Processing) //this is to stop the thread if stop button is pressed
                {
                    
                    DataAccess.Instance.g_WorkingFile = file;
                    try
                    {
                        string ext = Path.GetExtension(DataAccess.Instance.g_WorkingFile);
                        if (((string.Compare(ext, ".cbr") == 0) || (string.Compare(ext, ".cbz") == 0) || (string.Compare(ext, ".CBZ") == 0) || (string.Compare(ext, ".CBR") == 0)) && (_Cbr2Pdf))
                        {

                            ExtractSingleFile();
                        }
                        else if (((string.Compare(ext, ".pdf") == 0) || (string.Compare(ext, ".PDF") == 0)) && (_Pdf2Cbz))
                        {
                            CompressSingleFile();
                        }
                       
                        //updating the total progression bar
                        DataAccess.Instance.g_totProgress += singval;
                        evnt_UpdatTotBar(this, e);
                    }
                    catch (Exception a)
                    {
                    }
                }
            }

            //finished, update the ui
            DataAccess.Instance.g_Processing = false;
            DataAccess.Instance.g_totProgress = 0;
            DataAccess.Instance.g_WorkingFile = string.Empty;
            evnt_UpdateFileName(this, e);
            evnt_UpdatTotBar(this, e);
        }

        /// <summary>
        /// Generate the pdf from the images
        /// </summary>
        private void GeneratePdf()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            string[] imageFiles = Directory.GetFiles(temporaryDir);

            //count for progression bar
            CurOneStep =  imageFiles.Count();
            int divider;
            if (DataAccess.Instance.g_ReduceSize)
                divider = 33;
            else
                divider = 50;
            CurOneStep = divider / CurOneStep;
            

                //importing the images in the pdf
                foreach (string imageFile in imageFiles)
                {
                    if (DataAccess.Instance.g_Processing)
                    {
                        try
                        {
                            //updating progress bar
                            DataAccess.Instance.g_curProgress += CurOneStep;
                            evnt_UpdateCurBar();                           

                            if (IsSingleFile)
                            {
                                DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                                evnt_UpdatTotBar(this, e);
                            }

                            //checking file extension
                            string ext = Path.GetExtension(imageFile);
                            if ((string.Compare(ext, ".jpg") == 0) || (string.Compare(ext, ".jpeg") == 0) || (string.Compare(ext, ".png") == 0) || (string.Compare(ext, ".bmp") == 0) || (string.Compare(ext, ".new") == 0)
                                || (string.Compare(ext, ".JPG") == 0) || (string.Compare(ext, ".JPEG") == 0) || (string.Compare(ext, ".PNG") == 0) || (string.Compare(ext, ".BMP") == 0) || (string.Compare(ext, ".NEW") == 0))
                            {

                                // Create an empty page
                                PdfPage page = document.AddPage();
                                page.Size = PageSize.A4;
                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                using (XImage image = XImage.FromFile(imageFile))
                                {
                                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }
            
           
            //saving file
            string filename = temporaryDir + ".pdf";
            document.Save(filename);

        }

        /// <summary>
        /// Compress the image and reduce the quality
        /// </summary>
        private void CompressImage()
        {
            int NEW_IMG_QUALITY = 30; //i found that 30 is not too bad and the filesize is about the half

            string[] imageFiles = Directory.GetFiles(temporaryDir);

            //count for progress bar
            CurOneStep = imageFiles.Count();
            int divider;
            if (DataAccess.Instance.g_ReduceSize)
                divider = 33;
            else
                divider = 50;
            CurOneStep = divider / CurOneStep;

            //compress every image
            foreach (string imageFile in imageFiles)
            {
                if (DataAccess.Instance.g_Processing) //this is to stop the thread if the user press the stop button
                {

                    string NewImg;
                    //checking file extension
                    string ext = Path.GetExtension(imageFile);
                    if ((string.Compare(ext, ".jpg") != 0) && (string.Compare(ext, ".jpeg") != 0) &&  (string.Compare(ext, ".bmp") != 0)
                        &&(string.Compare(ext, ".JPG") != 0) && (string.Compare(ext, ".JPEG") != 0) && (string.Compare(ext, ".BMP") != 0))
                        break;

                    //compressing
                    using (Image OldImg = Image.FromFile(imageFile))
                    {
                         NewImg = imageFile + ".new";
                        SaveJpeg(NewImg, OldImg, NEW_IMG_QUALITY);
                    }

                    //updating progress bar
                    DataAccess.Instance.g_curProgress += CurOneStep;
                    evnt_UpdateCurBar();

                    if (IsSingleFile)
                    {
                        DataAccess.Instance.g_totProgress = DataAccess.Instance.g_curProgress;
                        evnt_UpdatTotBar(this, e);
                    }

                    //delete the old image
                    File.Delete(imageFile);

                    //remove the .new
                    File.Move(NewImg, imageFile);
                }
            }

        }

        /// <summary>
        /// Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        // <param name="quality">An integer from 0 to 100, with 100 being the
        /// highest quality</param>
        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            // Encoder parameter for image quality
            EncoderParameter qualityParam =    new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }


    }
}
