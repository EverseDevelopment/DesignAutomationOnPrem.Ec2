using System.IO;
using System.IO.Compression;

namespace DesignAutomationEc2.Actions
{
    public class ZipCompression
    {
        public static void Create(string sourceFolder, string destinationZipFilePath)
        {
            try
            {
                // Delete existing zip file if it exists
                if (File.Exists(destinationZipFilePath))
                {
                    File.Delete(destinationZipFilePath);
                }

                // Create a zip at the destination path
                ZipFile.CreateFromDirectory(sourceFolder, destinationZipFilePath);

                Console.WriteLine($"All files in '{sourceFolder}' have been zipped to '{destinationZipFilePath}'");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"An I/O error occurred: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
