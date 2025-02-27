using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Add this using directive
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add this using directive
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Secure the controller with the Authorize attribute
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>(); // In-memory list to store users

        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
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
                Id = _users.Count + 1, // Generate a new ID
                Name = userDto.Name,
                Email = userDto.Email
            };

            // Add user to the in-memory list
            _users.Add(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user); // Return the created user in the response body
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            // Update user in the in-memory list
            user.Name = userDto.Name;
            user.Email = userDto.Email;

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            // Remove user from the in-memory list
            _users.Remove(user);

            return NoContent();
        }
    }

    // User model
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; } // Use required modifier
        public required string Email { get; set; } // Use required modifier
    }

    // UserDto model
    public class UserDto
    {
        public required string Name { get; set; } // Use required modifier

        [EmailAddress(ErrorMessage = "Invalid email address")] // Add email validation
        public required string Email { get; set; } // Use required modifier
    }
}
