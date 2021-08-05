using System;
using System.Net;

using System.Threading.Tasks;

using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda
{
    public class Function
    {

        private const string BucketUri = "s3://amazon-transcribe-0001/audio/car-jp-5mins.mp3";
        private static string _bucketName;

       

        ///<summary>
        ///S3Util
        ///<summary>
        ///S3RdssUtil_s3Util;

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(string input, ILambdaContext context)
        {
            await CreateTranscribe();
        }

        static async Task CreateTranscribe()
        {

            var langCode = "ja-JP";
            var filename = "car-jp-5mins.mp3";

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
                    TranscriptionJobName = string.Format("transcribe-job-{0}-", _bucketName),
                    OutputBucketName = _bucketName,
                };

                var transcriptionJobResponse = await transcribeClient.StartTranscriptionJobAsync(transcriptionJobRequest);
                Console.WriteLine(transcriptionJobResponse);

                if (transcriptionJobResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Couldn't create transcription job");
                }
            }

            Console.WriteLine("The transcription job request has been created successfully.");
        }
    }
}
