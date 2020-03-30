using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExemploApiUsingJwt.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExemploApiUsingJwt.Api.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        [Route("api/token/create")]
        public IActionResult Create([FromBody]LoginViewModel model)
        {
            if (model.Email == "teste@gmail.com" && model.Password == "numsey")
            {
                return new ObjectResult(GenerateToken(model.Email));
            }
            return BadRequest();
        }

        private string GenerateToken(string email)
        {
            var claims = new Claim[]
            {
                //define as claims 
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            //gera o token
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("senhasupersecretaparaauth"));
            SigningCredentials signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader jwtHeader = new JwtHeader(signingCredential);
            JwtPayload jwtPayload = new JwtPayload(claims);
            JwtSecurityToken token = new JwtSecurityToken(jwtHeader, jwtPayload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}