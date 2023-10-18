using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Security
{
    public class JWTManager : IJWTManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        public JWTManager(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(IdentityUser user)
        {
            var config = _configuration.GetSection("JwtSettings");
            var secretKeyStr = config.GetValue<string>("SecretKey");
            var secretKey = Encoding.ASCII.GetBytes(secretKeyStr);
            var expiryDuration = config.GetValue<int>("ExpiresIn");

            var tokenHandler = new JwtSecurityTokenHandler();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>(){ 
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken
            ( 
                issuer: config["validIssuer"],
                audience: config["validAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryDuration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            );

            var res = tokenHandler.WriteToken(token);
            return res;
        }
    }
}
