using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using commentsAPI.Models;
using commentsAPI.Resources;
using commentsAPI.Services;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace commentsAPI.Controllers
{
    public class PostController : ApiController
    {
        private readonly string apiVersion = API.API_VERSION;

        [HttpGet]
        [Route("posts")]
        public IHttpActionResult GetPosts()
        {
            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");
            List<Post> postList = PostService.getPosts();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            return Json(postList, serializerSettings);
        }
        
        [HttpGet]
        [Route("posts/author/{author}")]
        public IHttpActionResult GetPosts(string author)
        {
            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");
            List<Post> postList = PostService.getPosts(author);

            if(postList != null)
            {
                HttpContext.Current.Response.Headers.Add("Post-Count", postList.Count + " Posts");
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                return Json(postList, serializerSettings);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("posts/{title}")]
        public IHttpActionResult GetPost(string title)
        {
            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");
            Post post = PostService.getPost(title);

            if (post != null)
            {
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                return Json(post, serializerSettings);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("posts")]
        public IHttpActionResult CreatePost(Post post)
        {
            Post createdPost = PostService.createPost(post);
            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");

            // There's not a post with that name in the database.
            if(createdPost != null)
            {
                string currentURL = HttpContext.Current.Request.Url.AbsoluteUri;
                string postLocation;

                if (currentURL.EndsWith("/"))
                {
                    postLocation = currentURL + createdPost.title;
                }
                else
                {
                    postLocation = currentURL + "/" + createdPost.title;
                }
                HttpContext.Current.Response.Headers.Add("Object-URL", postLocation);

                return Created(postLocation, createdPost);
            }
            // Error 409 - There's already a post with that name in the database.
            else
            {
                return Conflict();
            }
        }

        [HttpPut]
        [Route("posts/{title}")]
        public IHttpActionResult UpdatePost(string title, Post post)
        {
            Post updatedPost = PostService.updatePost(title, post);

            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");
            
            if (updatedPost != null)
            {
                string currentURL = HttpContext.Current.Request.Url.AbsoluteUri;
                string newURL = currentURL.Remove(currentURL.LastIndexOf('/'));
                string postLocation;

                if (currentURL.EndsWith("/"))
                {
                    postLocation = newURL + updatedPost.title;
                }
                else
                {
                    postLocation = newURL + "/" + updatedPost.title;
                }
                HttpContext.Current.Response.Headers.Add("Object-URL", postLocation);

                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                return Json(updatedPost, serializerSettings);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("posts/{title}")]
        public IHttpActionResult DeletePost(string title)
        {
            Post deletedPost = PostService.deletePost(title);
            HttpContext.Current.Response.Headers.Add("API-Version", apiVersion);
            HttpContext.Current.Response.Headers.Add("Response-Type", "JSON");

            if (deletedPost != null)
            {
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                return Json(deletedPost, serializerSettings);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
