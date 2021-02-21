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

namespace CommentSaver0221
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
            CommentQueueModel data = JsonConvert.DeserializeObject<CommentQueueModel>(requestBody);
            if (data != null)
            {
                commentQueue.Add(JsonConvert.SerializeObject(data));
            }

            string responseMessage = "This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);
        }
    }
}
