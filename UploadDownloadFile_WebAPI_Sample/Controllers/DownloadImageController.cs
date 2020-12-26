using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UploadDownloadFile_WebAPI_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadImageController : ControllerBase
    {
        //https://localhost:44325/api/DownloadImage/FILENAME.TYPEFILE      
        /// <summary>
        /// Download image from server
        /// </summary>
        /// <param name="filename">filename for download</param>
        /// <returns></returns>
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadIm(string filename)
        {
            var path = Path.GetFullPath("./wwwroot/upload/" + filename);
            MemoryStream memory = new MemoryStream(); //чтение вводимых данных из массива или запись выводимых данных в массив
            using (FileStream FileStream = new FileStream(path, FileMode.Open))
            {
                await FileStream.CopyToAsync(memory);
            }
            memory.Position = 0; //Текующая позиция в потоке (очистка)
            return File(memory, "image/jpg", Path.GetFileName(path));
        }
    }
}