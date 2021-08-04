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
        private const string BucketUri = "s3://amazon-transcribe-0001/short-news-jp.mp3";
        private static string _bucketName;

        static async Task Main()
        {

            var langCode = "ja-JP";
            var filename = "short-news-jp.mp3";

            _bucketName = "amazon-transcribe-0001";




            await TranscribeInputFile(filename, langCode);

            Console.WriteLine("The process is complete");
        }


        static async Task TranscribeInputFile(string fileName, string targetLanguageCode)
        {

            using (var transcribeClient = new AmazonTranscribeServiceClient(Amazon.RegionEndpoint.APNortheast1))
            {
                var media = new Media()
                {
                    MediaFileUri = BucketUri
                };

                 var transcriptionJobRequest = new StartTranscriptionJobRequest()
                {
                    LanguageCode = targetLanguageCode,
                    Media = media,
                    MediaFormat = MediaFormat.Mp3,
                    TranscriptionJobName = string.Format("transcribe-job-"+ fileName, _bucketName),
                    OutputBucketName = _bucketName,
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
