using System;
using CommentSaver0221.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommentSaver0221
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class WriteToTable
    {
        [FunctionName("WriteToTable")]
        [return: Table("comments")]
        public static CommentTableModel Run(
            [QueueTrigger("comment-queue")] CommentQueueModel myQueueItem, 
            [Queue("comments-queue-2")] ICollector<string> commentQueue,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed");

            var comment = new CommentTableModel(myQueueItem.PostId, Guid.NewGuid())
            {
                Comment = myQueueItem.Message,
                Author = myQueueItem.UserName,
                Status = false
            };
            commentQueue.Add(JsonConvert.SerializeObject(comment));
            log.LogInformation($"Comment added to comments-queue-2");
            return comment;
        }
    }
}
