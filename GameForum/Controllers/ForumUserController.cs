using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumUserController : ControllerBase
    {
        // GET: api/<ForumUserController>
        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = GameForum.User.selectAll();
            if(users.Count == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, users);
            }
        }

        // GET api/<ForumUserController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = GameForum.User.Select(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, user);
            }
        }

        // POST api/<ForumUserController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            GameForum.User.Create(user);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<ForumUserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if(GameForum.User.CheckExists(id))
            {
                GameForum.User.Update(id, user);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }

        // DELETE api/<ForumUserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (GameForum.User.CheckExists(id))
            {
                GameForum.User.Delete(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }
    }
}
