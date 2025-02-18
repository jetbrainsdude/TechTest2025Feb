using Lexxika.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lexxika.Documents.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lexxika.Documents.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorityController: ControllerBase
    {
        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            // hardcoded credentials for now
            if ((login.UserName == "admin" && login.Password == "password") ||
                (login.UserName == "user1" && login.Password == "password") ||
                (login.UserName == "user2" && login.Password == "password"))
            {
                var token = GenerateJwtToken(login.UserName);

                // return token to client
                return Ok(new { token });
            }
            
            return Unauthorized();
        }

        // GET: api/auth/data
        [HttpGet("data")]
        [Authorize]
        public ActionResult<UserClaims> GetAll()
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                return userClaims;
            }
            catch
            {
                // log error
                return NotFound();
            }
        }


        /// <summary>
        /// In response to a username and password being sent to the /token path, this method returns an 
        /// access token to the client which can be used in subsequent calls. Here access may be segregated 
        /// with admin or just user. 
        /// </summary>
        private string GenerateJwtToken(string userName)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserName", userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // =========================================================================
            // TODO determine from data source if incoming credentials are admin or user
            // =========================================================================

            // set the role of any found user into the claims for later use in the api
            if (userName == "admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_which_must_be_long_256_bytes"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


