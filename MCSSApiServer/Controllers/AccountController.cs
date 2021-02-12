using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MCSSApiServer.Models;
using System.Security.Cryptography;
using System.Text;

namespace MCSSApiServer.Controllers
{
    public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных
        private readonly DatabaseContext databaseContext;
        public AccountController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public static string ComputeSHA256Hash(string text)
        {
            using var sha256 = new SHA256Managed();
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        [HttpPost("/token")]
        public IActionResult Token([FromForm] string username, [FromForm] string password)
        {
            password = ComputeSHA256Hash(password);



            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = databaseContext.Users.Where(x => x.Login == username && x.Password == password).FirstOrDefault();

            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
