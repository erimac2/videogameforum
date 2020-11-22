using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GameForum.Models;
using GameForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumUserController : ControllerBase
    {
        private IUserService _userService;

        public ForumUserController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return StatusCode(StatusCodes.Status200OK, response);
        }
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            Entities.User.Logout(bearerId);
            return StatusCode(StatusCodes.Status200OK, new { message = "User logged out" });
        }
        // GET: api/<ForumUserController>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Get()
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if(!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            List<Entities.User> users = Entities.User.selectAll();
            if(users.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, users);
            }
        }
        // GET api/<ForumUserController>/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
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
            Entities.User user = Entities.User.Select(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, user);
            }
        }
        // GET api/<ForumUserController>/5/comments
        [AllowAnonymous]
        [HttpGet("{id}/comments")]
        public IActionResult Comments(int id)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            List<Comment> comments = Comment.SelectAllByUser(id);
            if(bearerId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, comments);
            }
        }
        // POST api/<ForumUserController>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] Entities.User user)
        {
            Entities.User.Create(user);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<ForumUserController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Entities.User user)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            if (bearerId != id)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Incorrect id" });
            }

            if (Entities.User.CheckExists(id))
            {
                Entities.User.Update(id, user);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        // DELETE api/<ForumUserController>/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            if (bearerId != id)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Incorrect id" });
            }
            if (Entities.User.CheckExists(id))
            {
                Entities.User.Delete(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
