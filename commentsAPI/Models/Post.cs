using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace commentsAPI.Models
{
    public class Post
    {
        [BsonId]
        public string title { get; set; }
        [BsonElement("post_date")]
        public DateTime postDate { get; set; } = DateTime.Now;
        [BsonElement("author")]
        public string author { get; set; }
        [BsonElement("body")]
        public string body { get; set; }
    }
}