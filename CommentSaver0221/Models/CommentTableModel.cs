using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommentSaver0221.Models
{
    public class CommentTableModel : TableEntity
    {
        public CommentTableModel(string postId, Guid commentId)
        {
            this.PartitionKey = postId;
            this.RowKey = commentId.ToString();
        }
        public string Author { get; set; }
        public string Comment { get; set; }
        public bool Status { get; set; }
    }
}
