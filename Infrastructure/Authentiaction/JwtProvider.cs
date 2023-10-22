using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentiaction
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration; 

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate(string email, Guid ObjectGuid)
        {
            var claims = new Claim[]
            {
            new(JwtRegisteredClaimNames.Email , email) , 
            new(ClaimTypes.PrimarySid , ObjectGuid.ToString())
            };

            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];

            var signingCredentials = new SigningCredentials( 
                new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer , 
                issuer ,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
