using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Utils
{
    public class Constants
    {
        public class AzureStorageImage
        {
            public static readonly string[] AllowUploadedImageFileTypes = {"jpg", "png", "gif", "jpeg"};
            public const int UploadedImageFileSize = 1;
        }
    }
}
