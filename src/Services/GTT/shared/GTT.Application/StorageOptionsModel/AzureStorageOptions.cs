using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.StorageOptionsModel
{
    public class AzureStorageOptions
    {
        public string AzureWebJobsStorage { get; set; }
        public string ContainerName { get; set; }
    }
}
