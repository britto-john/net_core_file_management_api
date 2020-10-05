using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileManagementAPI.Lib;

namespace FileManagementAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        private static string _folderName = "Documents";
        private readonly object _lock = new object();

        // GET: api
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string[] fileNames = { };
            try
            {
                var documentPath = Path.Combine(Directory.GetCurrentDirectory(), _folderName);
                FileInfo[] filePaths = new DirectoryInfo(documentPath).GetFiles();

                fileNames = filePaths.Select(m => m.Name).ToArray();
            }
            catch (Exception ex)
            {
                // ToDo - Log errors
                return StatusCode(500, $"Internal server error: {ex}");
            }

            return fileNames;
        }

        // GET api/filename
        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string filename)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), _folderName);
                FileInfo file = new FileInfo(path + "\\" + filename);
                if (file.Exists)
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(file.FullName, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;

                    return File(memory, FileHandler.GetContentType(file.FullName), Path.GetFileName(file.FullName));
                }
                else
                {
                    return NotFound("File not found - " + filename);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // POST api
        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                var file = Request.Form.Files[0];

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), _folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // DELETE api
        [HttpDelete("{filename}")]
        public IActionResult Delete(string filename)
        {
            try
            {
                lock (_lock)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), _folderName);
                    FileInfo file = new FileInfo(path + "\\" + filename);
                    if (file.Exists)
                    {
                        file.Delete();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // PUT api
        [HttpPut("{filename}")]
        public void Put(string filename, [FromBody] string newvalue)
        {
            // To Do
            throw new NotImplementedException("This method is not implemented");
        }


    }
}
