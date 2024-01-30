using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;

namespace DesignAutomationEc2.Actions
{
    public class S3
    {
        public static async Task UploadFileAsync(string filePath, string folderPath)
        {
            var s3Client = new AmazonS3Client(ConfigValues.AWS_AccessKey, ConfigValues.AWS_SecretKey, ConfigValues.regionEndpoint);
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                // TODO Make sure the filepath in AWSS3 changes every time a file is uploaded
                await fileTransferUtility.UploadAsync(filePath, ConfigValues.BucketName);
                var url = await GetPublicUrlAsync(s3Client, Path.GetFileName(filePath));
                Remove.AllFiles(folderPath);

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

        static async Task<string> GetPublicUrlAsync(AmazonS3Client s3Client, string objectKey)
        {
            var request = new Amazon.S3.Model.GetPreSignedUrlRequest
            {
                BucketName = ConfigValues.BucketName,
                Key = objectKey,
                Expires = DateTime.Now.AddHours(ConfigValues.LINK_DURATION_IN_HOURS),
                Protocol = Protocol.HTTPS
            };

            var url = s3Client.GetPreSignedURL(request);
            return url;
        }

        static void SendEmail(string recipientEmail, string URL)
        {
            string emailContent = $"This is the URL: {URL}";
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(ConfigValues.EMAIL_NAME, ConfigValues.SMTP_USER));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = ConfigValues.EMAIL_SUBJECT;
                message.Body = new TextPart("plain")
                {
                    Text = emailContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(ConfigValues.SMTP_SERVER, ConfigValues.SMTP_PORT, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(ConfigValues.SMTP_SENDER, ConfigValues.SMTP_PASSWORD);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(emailContent);
                Console.WriteLine(ex.Message);
            }
        }
    }

}