using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Enums;
using XdPagamentosApi.Shared.Extensions;

namespace XdPagamentosApi.Shared.Token
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
                    new Claim(ClaimTypes.Name, user.Id.ToString().Criptografar(TipoSistema.Admin)),
                    new Claim(ClaimTypes.NameIdentifier, user.Tipo)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHankder.CreateToken(tokenDescriptor);
            return tokenHankder.WriteToken(token);
        }

        public static string GenerateToken(DtoUsuarioClienteLogado user)
        {
            var tokenHankder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SettingsToken.SecretClient);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString().Criptografar(TipoSistema.Cliente)),
                    new Claim(ClaimTypes.NameIdentifier, user.Tipo)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHankder.CreateToken(tokenDescriptor);
            return tokenHankder.WriteToken(token);
        }
    }
}
