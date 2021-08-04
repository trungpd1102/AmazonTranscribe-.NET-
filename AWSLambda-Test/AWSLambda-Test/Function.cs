using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda_Test
{
    public class Function
    {
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
        public ResponseData FunctionHandler(Param input, ILambdaContext context)
        {
            var responseData = new ResponseData
            {
                result = false
            };

            try
            {
                var deviceId = HttpUtility.UrlDecode(input.deviceId);
                Console.WriteLine("===============OK================");
                Console.WriteLine($"deviceId: {deviceId}");

                //
                Console.WriteLine("=================================");

                responseData.result = true;
            }
            catch (Exception ex)
            {
                responseData.result = false;
                responseData.message = ex.ToString();
                return responseData;
            }
            return responseData;
        }

            public class ResponseData
            {
                public bool result { get; set; }

                public String message { get; set; }
            }

            public class Param
            {
                public string deviceId { get; set; }
            }
        }
    }

