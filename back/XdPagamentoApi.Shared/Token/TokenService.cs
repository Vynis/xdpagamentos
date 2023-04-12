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
        public static string GenerateToken(DtoUsuarioLogado user, TipoSistema tipoSistema = TipoSistema.Admin)
        {
            var tokenHankder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tipoSistema == 0 ? SettingsToken.Secret : SettingsToken.SecretClient);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString().Criptografar(tipoSistema == 0 ? TipoSistema.Admin : TipoSistema.Cliente)),
                    new Claim(ClaimTypes.NameIdentifier, user.Tipo)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHankder.CreateToken(tokenDescriptor);
            return tokenHankder.WriteToken(token);
        }
    }
}
