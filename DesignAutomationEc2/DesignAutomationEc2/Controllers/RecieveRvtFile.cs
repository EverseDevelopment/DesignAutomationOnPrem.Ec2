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

            var folderPath = "C:\\Users\\User\\Desktop\\Test"; // Specify your folder path here
            var filePath = Path.Combine(folderPath, file.FileName);

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
            string exePath = @"C:\Program Files\Autodesk\Revit 2023\RevitExtractor\RevitExtractor.exe";
            string sourcePath = @"C:\Users\User\Desktop\Test";

            // Construct the full command with arguments
            string arguments = $"\"{sourcePath}\" \"{filePath}\"";

            Task.Run(() => ProcessFileInBackground(exePath, arguments));

            return Ok($"File {file.FileName} has been uploaded successfully.");
        }

        private void ProcessFileInBackground(string exePath, string arguments)
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

            string svfPath = @"C:\Users\User\Desktop\Test\output";
            ZipCompression.Create(svfPath, @"C:\Users\User\Desktop\Test\svffile.zip");
            S3.UploadFileAsync(@"C:\Users\User\Desktop\Test\svffile.zip");
            
        }
    }
}


