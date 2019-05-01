using commentsAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace commentsAPI.Services
{
    public static class PostService
    {
        public static List<Post> getPosts()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");
            List<Post> postList = collection.Find(_ => true).ToList();

            return postList;
        }

        public static Post getPost(string title)
        {
            var postTitle = title.Trim().ToUpperInvariant();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");

            Post foundPost = collection.Find(x => x.title.ToUpperInvariant() == postTitle).SingleOrDefault();

            if (foundPost != null)
            {
                return foundPost;
            }
            else
            {
                return null;
            }
        }

        public static List<Post> getPosts(string author)
        {
            string authorName = author.ToUpperInvariant().Trim();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");

            List<Post> postList = collection.Find(x => x.author.ToUpperInvariant() == authorName).ToList();

            if (postList.Count >= 1)
            {
                return postList;
            }
            else
            {
                return null;
            }
        }

        public static Post createPost(Post post)
        {
            string postTitle = post.title.Trim().ToUpperInvariant();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");

            // Object corrections
            post.author = post.author.Trim();
            post.body = post.body.Trim();

            Post foundPost = collection.Find(x => x.title.ToUpperInvariant() == postTitle).FirstOrDefault();
            if(foundPost != null)
            {
                return null;
            }
            else
            {
                collection.InsertOne(post);
                return post;
            }
        }

        public static Post updatePost(string title, Post post)
        {
            string postTitle = title.Trim().ToUpperInvariant();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");

            // Object corrections
            post.author = post.author.Trim();
            post.body = post.body.Trim();

            Post foundPost = collection.Find(x => x.title.ToUpperInvariant() == postTitle).FirstOrDefault();

            // If post hasn't been found in database, return null.
            if (foundPost == null)
            {
                return null;
            }
            else
            {
                collection.DeleteOne(x => x.title.ToUpperInvariant() == postTitle);
                collection.InsertOne(post);
                return post;
            }
            
        }

        public static Post deletePost(string title)
        {
            string postTitle = title.Trim().ToUpperInvariant();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("postDB");
            IMongoCollection<Post> collection = db.GetCollection<Post>("posts");

            Post foundPost = collection.Find(x => x.title.ToUpperInvariant() == postTitle).FirstOrDefault();

            if (foundPost != null)
            {
                collection.DeleteOne(x => x.title.ToUpperInvariant() == postTitle);
                return foundPost;
            }
            else
            {
                return null;
            }
        }


    }
}