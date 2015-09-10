using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CbrConverter
{
    public class PdfExtractionResult
    {
        public int Pages { get; set; }
        public int ImagesBeforeMerge { get; set; }
        public int ImagesAfterMerge { get; set; }
    }
}
