using KeitaMigration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace KeitaMigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        
        public TokenController(IConfiguration configuration, AppDbContext appdbcontext)
        {
            _configuration = configuration;
            _appDbContext = appdbcontext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] Usuario request)
        {
            var dbEntry = _appDbContext.Usuarios.FirstOrDefault(acc => (acc.Nome == request.Nome && acc.Senha == request.Senha));

            if (dbEntry != null)
            {
                var claims = new[]
                 {
                    new Claim(ClaimTypes.Name, request.Nome)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "yujiissuer",
                    audience: "yujiaudience",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });

            }
            return BadRequest("Credenciais Invalidas");
        }

    }
}
