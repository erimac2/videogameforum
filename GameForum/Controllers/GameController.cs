using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GameForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IUserService _userService;

        public GameController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<GameController>
        [AllowAnonymous]
        public IActionResult Get()
        {
            List<Game> games = Game.selectAll();
            if (games.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, games);
            }
        }

        // GET api/<GameController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Game game = Game.Select(id);
            if (game == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, game);
            }
        }
        // GET api/<GameController>/5/posts
        [AllowAnonymous]
        [HttpGet("{id}/posts")]
        public IActionResult Posts(int id)
        {
            List<Post> posts = GameForum.Post.SelectAllInGame(id);

            if (posts.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, posts);
            }
        }
        // POST api/<GameController>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            Game.Create(game);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<GameController>/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Game game)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            int bearerId = _userService.GetId(accessToken);
            if (!Entities.User.IsOnline(bearerId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User not logged in" });
            }
            if (Game.CheckExists(id))
            {
                Game.Update(id, game);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        // DELETE api/<GameController>/5
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
            if (Game.CheckExists(id))
            {
                Game.Delete(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}
