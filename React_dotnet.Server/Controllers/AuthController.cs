using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using React_dotnet.database;
using React_dotnet.database.Models;
using React_dotnet.Server.Dtos;
using React_dotnet.Server.Dtos.Auth;
using React_dotnet.Server.Dtos.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace React_dotnet.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase    
    {
        private readonly IOptions<JwtOptions> options;
        private readonly CoreDbContext coreDbContext;

        public AuthController(IOptions<JwtOptions> options, CoreDbContext coreDbContext)
        {
            this.options = options;
            this.coreDbContext = coreDbContext;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult Register(UserDto userDto) // Kézi mappelés
        {
            var hasher = new PasswordHasher<object>();
            var passwordHash = hasher.HashPassword(null!, userDto.Password);

            // Kézi mappelés (mapper pl: Automapper)
            var user = new User
            {
                Id = 0,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = passwordHash

            };

            coreDbContext.Users.Add(user);
            coreDbContext.SaveChanges();

            return NoContent();

        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login(LoginRequest request)
        {
            var user = coreDbContext
                .Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefault(u => u.Email == request.Email);
            
            if (user == null)
            {
                return Unauthorized();
            }

            var hasher = new PasswordHasher<object>();

            var result = hasher.VerifyHashedPassword(null!, user.PasswordHash, request.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            var accessToken = GenerateJwtToken(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpiresInMinutes),
                SameSite = SameSiteMode.Lax
            };

            HttpContext.Response.Cookies.Append(Constants.AccessTokenCookieKey, accessToken, cookieOptions);

            return Ok(accessToken);
        }

        [Authorize]
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(Constants.AccessTokenCookieKey);

            return Ok();
        }

        [Authorize]
        [HttpGet("cookietoken")]
        public async Task<ActionResult<string>> GetTokenFromCookie()
        {
            Request.Cookies.TryGetValue(Constants.AccessTokenCookieKey, out string? accessToken);

            if(string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized();
            }

            return Ok(accessToken);
        }



        private string GenerateJwtToken(User user)
        {
           var jwt = options.Value;

            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.Email), // Ez kerül majd a HttpContecxt.User.Isdentity.Name - be
               new Claim(ClaimTypes.Email, user.Email),
               new Claim("firstName", user.FirstName),
               new Claim("lastName", user.LastName),
            };

            claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.ExpiresInMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
