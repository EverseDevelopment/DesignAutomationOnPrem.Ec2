using Amazon;

namespace DesignAutomationEc2
{
    public class ConfigValues
    {
        /// <summary>
        /// Set your Bucket Region
        /// </summary>
        public static readonly RegionEndpoint regionEndpoint = RegionEndpoint.USEast1;

        /// <summary>
        /// Set your Bucket name
        /// </summary>
        public const string BucketName = "everse.assets"; // Replace with your bucket name

        /// <summary>
        /// SET your Revit Extractor location
        /// </summary>
        public const string REVIT_EXTRACTOR_LOCATION = @"C:\Program Files\Autodesk\Revit 2023\RevitExtractor\RevitExtractor.exe"; // 

        /// <summary>
        /// Set the link duration in hours (default 1 hour)
        /// </summary>
        public const Int32 LINK_DURATION_IN_HOURS = 1;


        /// <summary>
        /// set SMTP server address
        /// </summary>
        public const string SMTP_SERVER = "smtp.google.com";

        /// <summary>
        /// set SMTP server port (25 default)
        /// </summary>
        public const int SMTP_PORT = 25;

        /// <summary>
        /// Set SMTP server user
        /// </summary>
        public const string SMTP_USER = "";

        /// <summary>
        /// Set SMTP server password
        /// </summary>
        public const string SMTP_PASSWORD = "";

        /// <summary>
        /// Set the sender for the email
        /// </summary>
        public const string SMTP_SENDER = "";

        /// <summary>
        /// set the email subject
        /// </summary>
        public const string EMAIL_SUBJECT = "New file in S3";

        /// <summary>
        /// Sender name for email
        /// </summary>
        public const string EMAIL_NAME = "";

        // -- AWS
        /// <summary>
        /// Set your AWS access key
        /// </summary>
        public static string AWS_AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"); // Replace with your access key

        /// <summary>
        /// Set your AWS secret key
        /// </summary>
        public static string AWS_SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"); // Replace with your secret key
    }
}
