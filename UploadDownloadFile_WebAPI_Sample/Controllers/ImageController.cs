using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadDownloadFile_WebAPI_Sample.Models;

namespace UploadDownloadFile_WebAPI_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Uploading selected image on server
        /// </summary>
        /// <param name="file">file that was selected for upload</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file != null)
            {
                // готовим структуру данных для операции
                string root = _environment.WebRootPath;
                string folder = "\\Upload\\";
                string path = root + folder;
                string fileName = file.FileName;

                // готовим окружение для операции
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // выполняем операцию
                await using (FileStream FileStream = System.IO.File.Create(path + fileName))
                {
                    file.CopyTo(FileStream);

                    try
                    {
                        FileStream.Flush();// Очищает буферы для этого потока и вызывает запись всех буферизованных данных в файл
                        string result = fileName + DateTime.Now.ToLongDateString();

                        return new StatusCodeResult(200);
                    }
                    catch (FileNotFoundException)
                    {
                        return new StatusCodeResult(404);
                    }
                    catch (PathTooLongException)
                    {
                        return new StatusCodeResult(500);
                    }
                }
            }
            else
                return new StatusCodeResult(204);
        }


        //https://localhost:44325/api/DownloadImage/FILENAME.TYPEFILE      
        /// <summary>
        /// Downloading selected image from server
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