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
    public class UploadImageController
    {
        public static IWebHostEnvironment _environment;
        public UploadImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //https://localhost:44325/api/UploadImage
        /// <summary>
        /// Chose image from directory and upload it on server
        /// </summary>
        /// <param name="objFile">file that was selected for upload</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ImageUpload([FromForm] UploadImage_Model objFile)
        {
            if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");

            await using (FileStream FileStream = System.IO.File.Create
                (_environment.WebRootPath + "\\Upload\\" + objFile.File.FileName))
            {
                objFile.File.CopyTo(FileStream);
                FileStream.Flush();// Очищает буферы для этого потока и вызывает запись всех буферизованных данных в файл
                return "added file " + objFile.File.FileName;
            }
        }
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
                await using (FileStream FileStream = File.Create(path + fileName))
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
            {
                return new StatusCodeResult(204);
            }
        }
    }

}
