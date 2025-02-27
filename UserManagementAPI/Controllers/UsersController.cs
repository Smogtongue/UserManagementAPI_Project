using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            // ...code to get users...
            return Ok(new List<string> { "User1", "User2" });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            // ...code to get a user by id...
            return Ok("User" + id);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult CreateUser([FromBody] string user)
        {
            // ...code to create a new user...
            return CreatedAtAction(nameof(GetUser), new { id = 1 }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] string user)
        {
            // ...code to update a user...
            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            // ...code to delete a user...
            return NoContent();
        }
    }
}
