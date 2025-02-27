using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            // Simulate fetching users from a database
            var users = new List<User>
            {
                new User { Id = 1, Name = "User1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User2", Email = "user2@example.com" }
            };

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            // Simulate fetching a user by id from a database
            var user = new User { Id = id, Name = "User" + id, Email = "user" + id + "@example.com" };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map UserDto to User model
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email
            };

            // Add user to the database (pseudo code)
            // _context.Users.Add(user);
            // _context.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Simulate updating a user in the database
            var user = new User { Id = id, Name = userDto.Name, Email = userDto.Email };

            if (user == null)
            {
                return NotFound();
            }

            // Update user in the database (pseudo code)
            // _context.Users.Update(user);
            // _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            // Simulate fetching a user by id from a database
            var user = new User { Id = id, Name = "User" + id, Email = "user" + id + "@example.com" };

            if (user == null)
            {
                return NotFound();
            }

            // Delete user from the database (pseudo code)
            // _context.Users.Remove(user);
            // _context.SaveChanges();

            return NoContent();
        }
    }
}
