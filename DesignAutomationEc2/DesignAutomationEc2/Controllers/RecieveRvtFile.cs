using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using DesignAutomationEc2.Actions;

namespace DesignAutomationEc2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var allowedExtension = ".rvt";
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (fileExtension != allowedExtension)
                return BadRequest("Invalid file type. Only .rvt files are allowed.");

            var folderPath = Path.GetTempPath();
            var filePath = Path.Combine(folderPath, getNewName(file.FileName));

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

            // Offload the processing to a background thread
            string exePath = ConfigValues.REVIT_EXTRACTOR_LOCATION;

            // Construct the full command with arguments
            string arguments = $"\"{folderPath}\" \"{filePath}\"";

            Task.Run(() => ProcessFileInBackground(exePath, arguments, folderPath));

            return Ok($"File {file.FileName} has been uploaded successfully.");
        }

        private void ProcessFileInBackground(string exePath, string arguments, string tempFolderPath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(exePath, arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.Start();

                // Read the output from the command
                using (StreamReader sr = process.StandardOutput)
                {
                    string result = sr.ReadToEnd();
                    Console.WriteLine(result);
                }

                process.WaitForExit(); // Wait for the process to complete
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            string svfPath = @$"{tempFolderPath}\output";
            ZipCompression.Create(svfPath, @$"{tempFolderPath}\svffile.zip");
            S3.UploadFileAsync(@$"{tempFolderPath}\svffile.zip", tempFolderPath);
        }

        static string getNewName(string fullPath)
        {
            long milisegundos = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string extension = Path.GetExtension(fullPath);

            string newName = $"{milisegundos}{extension}";

            return newName;
        }
    }
}


