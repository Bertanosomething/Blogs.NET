using APP.Users.Domain;
using CORE.APP.Features;
using System.Globalization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;



namespace APP.Users.Features
{
    public class UsersDbHandler : Handler
    {
        protected readonly UsersDb _db;

        public UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
        protected virtual List<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString())
            };
        }

        protected virtual string CreateAccessToken(List<Claim> claims, DateTime expiration)
        {
            var signingCredentials = new SigningCredentials(AppSettings.SigningKey, SecurityAlgorithms.HmacSha256Signature);
            var jwtSecurityToken = new JwtSecurityToken(AppSettings.Issuer, AppSettings.Audience, claims, DateTime.Now, expiration, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }
    }
}