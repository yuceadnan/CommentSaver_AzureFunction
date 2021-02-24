using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CommentSaver0221.Models;

namespace CommentSaver
{
    public static class AddCommentQueue
    {
        [FunctionName("AddCommentQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "comment")] HttpRequest req,
            [Queue("comment-queue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> commentQueue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CommentQueueModel comment = JsonConvert.DeserializeObject<CommentQueueModel>(requestBody);

            if (comment != null)
            {
                commentQueue.Add(JsonConvert.SerializeObject(comment));
                log.LogInformation("Comment saved successfully.");
            }

            //json dosyasında özel bir parametreye erişim
            //var aa = Environment.GetEnvironmentVariable("myKey");

            string responseMessage = "This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);
        }
    }
}
