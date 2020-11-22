using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using GameForum.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private IUserService _userService;

        public CommentController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<CommentController>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Get()
        {
            List<Comment> comments = Comment.SelectAll();
            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, comments);
            }
        }

        // GET api/<CommentController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Comment comment = Comment.Select(id);
            if (comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, comment);
            }
        }
        // POST api/<CommentController>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            Comment.Create(comment);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<CommentController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
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
            if (Comment.CheckExists(id))
            {
                Comment.Update(id, comment);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        // DELETE api/<CommentController>/5
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
            Comment comment = GameForum.Comment.Select(id);
            if (bearerId != comment.fk_user)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Incorrect id" });
            }
            if (Comment.CheckExists(id))
            {
                Comment.Delete(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
