using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using CommentSaver0221.Models;

namespace CommentSaver
{
    //connection bilgisi global set edilebiliyor
    [StorageAccount("AzureWebJobsStorage")]
    public static class WriteToTable
    {
        [FunctionName("WriteToTable")]
        public static void Run(
            [QueueTrigger("comment-queue", Connection = "AzureWebJobsStorage")]CommentQueueModel myQueueItem,
            [Table(tableName: "comments")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed");
            var comment = new CommentTableModel(myQueueItem.PostId, Guid.NewGuid())
            {
                Author = myQueueItem.UserName,
                Comment = myQueueItem.Message,
                Status = false
            };

            cloudTable.CreateIfNotExists();
            TableOperation operation = TableOperation.Insert(comment);
            TableResult result = cloudTable.Execute(operation);

            log.LogInformation("Comment added to table");
        }
    }
}
