using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using GameServerApi.Models;

namespace GameServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserContext _context;
        public UserController(UserContext ctx)
        {
            _context = ctx;
        }
    
        // liste temporaire
        private static List<User> users = new List<User>
        {
            new User("admin", "adminpass", Role.ADMIN),
            new User("user1", "user1pass", Role.USER)
        };
        // GET: api/<UserController>/All
        [HttpGet("All")]
        public List<UserPublic> GetAllUsers()
        {
            // Return public view including Id
            return users.Select(u => new UserPublic(u.Id, u.Pseudo, u.UserRole)).ToList();
        }

        // GET api/<UserController>/{id}
        [HttpGet("{id}")]
        public UserPublic GetUserById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return new UserPublic(0, "Unknown", Role.USER); // or handle not found case as needed
            }
            return new UserPublic(user.Id, user.Pseudo, user.UserRole);
        }

        // POST api/<UserController>
        [HttpPost("Register")]
        public UserPublic RegisterUser(UserInfo newUser)
        {
            users.Add(new User(newUser.Pseudo, newUser.Mdp, Role.USER));
            return new UserPublic(newUser.Pseudo, Role.USER);
        }

        // POST api/<UserController>
        [HttpPost("Login")]
        public UserPublic Login(UserInfo userInfo)
        {
            // Use VerifyPassword to avoid accessing private password field directly
            var user = users.FirstOrDefault(u => u.Pseudo == userInfo.Pseudo && u.VerifyPassword(userInfo.Mdp));
            if (user != null)
            {
                return new UserPublic(user.Id, user.Pseudo, user.UserRole);
            }
            return new UserPublic(0, "Unknown", Role.USER);
        }




        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
