using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Configuracao.Token
{
    public static class TokenService
    {
        public static string GenerateToken(DtoUsuarioLogado user)
        {
            var tokenHankder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SettingsToken.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString().Criptografar())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var token = tokenHankder.CreateToken(tokenDescriptor);
            return tokenHankder.WriteToken(token);
        }
    }
}
