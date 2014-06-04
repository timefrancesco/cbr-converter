using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpCompress;
using SharpCompress.Archive;
using SharpCompress.Common;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using System.IO;
using System.Drawing;

namespace CbrConverter
{
    public class PdfFunctions
    {
        static string _ExtractionDir;
        public static event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();

        public static bool PDF_ExportImage(string filename, string dirForExtractions, int divider)
        {
           
            PdfDocument document = PdfReader.Open(filename);
            DataAccess.Instance.g_curProgress = 0;

            _ExtractionDir = dirForExtractions;

            int imageCount = 0;
            double tem0 = divider;
            double pgc = document.Pages.Count;
            double CurOneStep = (double)(tem0 / pgc);

            // Iterate pages
            foreach (PdfPage page in document.Pages)
            {
                // Get resources dictionary
                PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                if (resources != null)
                {
                    // Get external objects dictionary
                    PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
                    if (xObjects != null)
                    {
                        ICollection<PdfItem> items = xObjects.Elements.Values;
                        // Iterate references to external objects
                        foreach (PdfItem item in items)
                        {
                            PdfReference reference = item as PdfReference;
                            if (reference != null)
                            {
                                PdfDictionary xObject = reference.Value as PdfDictionary;
                                // Is external object an image?
                                if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                                {
                                    if (!ExportImage(xObject, ref imageCount))
                                        return false;
                                                                       
                                }
                            }
                        }
                    }
                }
                DataAccess.Instance.g_curProgress += CurOneStep;
                evnt_UpdateCurBar();
            }

            return true;


        }


        static bool ExportImage(PdfDictionary image, ref int count)
        {
            bool ret = false;
            try
            {
                string filter = image.Elements.GetName("/Filter");
            
                switch (filter)
                {
                    case "/DCTDecode":
                        ret = ExportJpegImage(image, ref count);
                        break;
                    /* case "/FlateDecode":
                            ExportAsPngImage(image, ref count);
                            break;*/
                    default:
                        ret = false;
                        break;
                }
            }
            catch (Exception)//in case its an object array so we need first to decompress it
            {
                byte[] stream = image.Stream.Value;

                PdfSharp.Pdf.Filters.FlateDecode aug = new PdfSharp.Pdf.Filters.FlateDecode();
                stream = aug.Decode(stream);
                ret = ExportJpegImage(stream, ref count);
            }

            return ret;
        }


        static bool ExportJpegImage(PdfDictionary image, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            byte[] stream = image.Stream.Value;
            string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
            //FileStream fs = new FileStream(tmp, FileMode.Create, FileAccess.Write);     

            using (FileStream fs = File.Open(tmp, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(stream);
                bw.Close();
            }
            return true;
        }


        static bool ExportJpegImage(byte[] stream, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file. 
            string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
            //FileStream fs = new FileStream(tmp, FileMode.Create, FileAccess.Write);     

            using (FileStream fs = File.Open(tmp, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(stream);
                bw.Close();
            }


            return true;
        }

    }

   
    
}
