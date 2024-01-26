namespace DesignAutomationEc2.Actions
{
    public class Remove
    {
        public static void AllFiles(string folderPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);

                // Delete all files
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                // Delete all subdirectories
                foreach (DirectoryInfo subDir in di.GetDirectories())
                {
                    subDir.Delete(true); // true means it will delete recursively
                }

                Console.WriteLine("All files and folders have been deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}

