using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadDownloadFile_WebAPI_Sample.Models
{
    public class UploadImage_Model
    {
        public IFormFile File { get; set; }
    }
}