using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace DesignAutomationEc2.Actions
{
    public class S3
    {
    private static string bucketName = "everse.assets"; // Replace with your bucket name

    // Specify your AWS credentials
    private static string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"); // Replace with your access key
    private static string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"); // Replace with your secret key

    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1; // Set your bucket region

    public static async Task UploadFileAsync(string filePath)
    {
        var s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        try
        {
            var fileTransferUtility = new TransferUtility(s3Client);

                                                                         // TODO Make sure the filepath in AWSS3 changes every time a file is uploaded
            await fileTransferUtility.UploadAsync(filePath, bucketName); // TODO obtain S3 bucket URL and send it on an email
            Remove.AllFiles(@"C:\Users\User\Desktop\Test");

            Console.WriteLine("File uploaded successfully!");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error encountered on server. Message:'{e.Message}'");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown encountered on server. Message:'{e.Message}'");
        }
    }
}

}