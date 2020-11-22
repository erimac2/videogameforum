using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GameForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameForum.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IUserService _userService;

        public PostController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<PostController>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Get()
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            List<Post> post = GameForum.Post.SelectAll();
            if (post.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, post);
            }
        }

        // GET api/<PostController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Post post = GameForum.Post.Select(id);
            if (post == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, post);
            }
        }
        // GET api/<PostController>/5/comments
        [AllowAnonymous]
        [HttpGet("{id}/comments")]
        public IActionResult Comments(int id)
        {
            List<Comment> comments = Comment.SelectAllInPost(id);

            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, comments);
            }
        }
        // POST api/<PostController>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Post post)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            GameForum.Post.Create(post);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<PostController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Post post)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            if(bearerId != id)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Incorrect id" });
            }
            if (GameForum.Post.CheckExists(id))
            {
                GameForum.Post.Update(id, post);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        // DELETE api/<PostController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            Post post = GameForum.Post.Select(id);
            if (bearerId != post.fk_user)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Incorrect id" });
            }
            if (GameForum.Post.CheckExists(id))
            {
                GameForum.Post.Delete(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
