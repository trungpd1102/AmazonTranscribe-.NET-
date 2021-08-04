using System;
using System.IO;
using System.Net;

using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;


namespace DemoAmazonTranscribe
{
    class Program
    {
        private const string BucketUri = "https://s3.eu-west-1.amazonaws.com/{0}/{1}";
        private static string _bucketName;

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please provide audio file and language code.");

                return;
            }

            var filename = args[0];
            var langCode = args[1];

            _bucketName = "assets-" + Guid.NewGuid().ToString();

            await UploadInputFileToS3(filename);

            await TranscribeInputFile(filename, langCode);

            Console.WriteLine("The process is complete");
        }

        static async Task UploadInputFileToS3(string fileName)
        {
            var objectName = Path.GetFileName(fileName);

            using (var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.APNortheast1))
            {
                var putBucketRequest = new PutBucketRequest()
                {
                    BucketName = _bucketName
                };

                var putBucketResponse = await s3Client.PutBucketAsync(putBucketRequest);

                if (putBucketResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Couldn't create the S3 bucket!");
                }

                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectName,
                    ContentType = "audio/mpeg",
                    FilePath = fileName
                };

                await s3Client.PutObjectAsync(putObjectRequest);
            }
        }

        static async Task TranscribeInputFile(string fileName, string targetLanguageCode)
        {
            var objectName = Path.GetFileName(fileName);

            using (var transcribeClient = new AmazonTranscribeServiceClient(Amazon.RegionEndpoint.APNortheast1))
            {
                var media = new Media()
                {
                    MediaFileUri = string.Format(BucketUri, _bucketName, objectName)
                };

                var transcriptionJobRequest = new StartTranscriptionJobRequest()
                {
                    LanguageCode = targetLanguageCode,
                    Media = media,
                    MediaFormat = MediaFormat.Mp3,
                    TranscriptionJobName = string.Format("transcribe-job-{0}", _bucketName),
                    OutputBucketName = _bucketName
                };

                var transcriptionJobResponse = await transcribeClient.StartTranscriptionJobAsync(transcriptionJobRequest);

                if (transcriptionJobResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Couldn't create transcription job");
                }
            }

            Console.WriteLine("The transcription job request has been created successfully.");
        }
    }
}
