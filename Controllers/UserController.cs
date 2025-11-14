using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

        // GET: api/<UserController>/All
        [HttpGet("All")]
        public async Task<ActionResult<List<UserPublic>>> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserPublic(u.Id, u.Pseudo, u.UserRole))
                .ToListAsync();

            return Ok(users);
        }

        // GET api/<UserController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPublic>> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserPublic(u.Id, u.Pseudo, u.UserRole))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUser(UserPass newUser)
        {
            var hasher = new PasswordHasher<User>();
            // on créer une nouvelle confiture avec les informations reçu
            User user = new User(newUser.Pseudo, "", Role.USER);
            user.Password = hasher.HashPassword(user, newUser.Password);
            // on l'ajoute a notre contexte (BDD)
            _context.Users.Add(user);
            // on enregistre les modifications dans la BDD ce qui remplira le champ Id de notre objet
            await _context.SaveChangesAsync();
            // on retourne un code 201 pour indiquer que la création a bien eu lieu
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // POST api/<UserController>
        [HttpPost("Login")]
        public async Task<ActionResult<UserPublic>> Login(UserPass userPass)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Pseudo == userPass.Pseudo);

            // non trouvé ou mot de passe incorrect
            if (user == null || !user.VerifyPassword(userPass.Password))
            {
                return Unauthorized(new { message = "Pseudo ou mot de passe incorrect" });
            }

            // si tout est bon, on retourne les infos publiques
            var userPublic = new UserPublic(user.Id, user.Pseudo, user.UserRole);
            return Ok(userPublic);
        }




        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        // maj du user
        public async Task<ActionResult<UserPublic>> PutUser(int id, [FromBody] UserUpdate userUpdate)
        {
            // Vérifier que l'id de l'URL correspond à un utilisateur
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Utilisateur avec id={id} non trouvé" });
            }

            try
            {
                if (!string.IsNullOrEmpty(userUpdate.NewPseudo))
                {
                    // Mise à jour du pseudo
                    user.Pseudo = userUpdate.NewPseudo;
                }

                if (userUpdate.NewUserRole.HasValue)
                {
                    // Mise à jour du rôle
                    user.UserRole = userUpdate.NewUserRole.Value;
                }

                // Si un changement de mot de passe est demandé
                if (!string.IsNullOrEmpty(userUpdate.NewPassword))
                {
                    if (string.IsNullOrEmpty(userUpdate.CurrentPassword))
                    {
                        return BadRequest(new { message = "Le mot de passe actuel est requis" });
                    }

                    if (!user.VerifyPassword(userUpdate.CurrentPassword))
                    {
                        return BadRequest(new { message = "Mot de passe actuel incorrect" });
                    }

                    // Mettre à jour le mot de passe
                    user.UpdatePassword(userUpdate.CurrentPassword, userUpdate.NewPassword);
                }

                // Sauvegarder les changements
                await _context.SaveChangesAsync();

                // Retourner la version publique (DTO) de l'utilisateur
                var userPublic = new UserPublic(user.Id, user.Pseudo, user.UserRole);
                return Ok(userPublic);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Erreur de concurrence lors de la mise à jour" });
            }
        }

        // DELETE api/<UserController>/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            // Rechercher l'utilisateur par son ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = $"Utilisateur avec id={id} non trouvé" });
            }

            try
            {
                // Supprimer l'utilisateur du contexte
                _context.Users.Remove(user);

                // Sauvegarder les modifications dans la base de données
                await _context.SaveChangesAsync();

                // Retourner un code 204 (No Content) pour indiquer que la suppression a réussi
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Gérer les erreurs de base de données (contraintes, etc.)
                return StatusCode(500, new { message = "Erreur lors de la suppression de l'utilisateur", details = ex.Message });
            }
        }
    }
}
