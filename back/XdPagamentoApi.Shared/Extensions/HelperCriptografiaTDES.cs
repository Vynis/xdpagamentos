using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Enums;
using XdPagamentosApi.Shared.Token;

namespace XdPagamentosApi.Shared.Extensions
{
    public static class HelperCriptografiaTDES
    {
        private static byte[] _chave = Encoding.UTF8.GetBytes(SettingsToken.Secret);
        private static byte[] _chaveCliente = Encoding.UTF8.GetBytes(SettingsToken.SecretClient);

        /// <summary>
        /// Método para realizar a criptografia usando o algorítmo TriploDes
        /// </summary>
        /// <param name="valor"> Valor que será criptografado</param>
        /// <returns>Valor criptografado</returns>
        public static string Criptografar(this string valor, TipoSistema tipoSistema = TipoSistema.Admin)
        {
            using (var hashprovider = new MD5CryptoServiceProvider())
            {
                var encriptar = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Key = hashprovider.ComputeHash(tipoSistema == 0 ? _chave : _chaveCliente),
                    Padding = PaddingMode.PKCS7
                };

                using (var transform = encriptar.CreateEncryptor())
                {
                    var dados = Encoding.UTF8.GetBytes(valor);
                    return Convert.ToBase64String(transform.TransformFinalBlock(dados, 0, dados.Length));
                }

            }
        }
        /// <summary>
        /// Método usando algorítimo de criptografia Triplo Des
        /// </summary>
        /// <param name="valor">Valor que será Descriptografado</param>
        /// <returns>Valor que será descriptografado</returns>
        public static string Descriptar(this string valor, TipoSistema tipoSistema = TipoSistema.Admin)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var descriptografar = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Key = hashProvider.ComputeHash(tipoSistema == 0 ? _chave : _chaveCliente),
                    Padding = PaddingMode.PKCS7
                };
                using (var transforme = descriptografar.CreateDecryptor())
                {
                    var dados = Convert.FromBase64String(valor.Replace(" ", "+"));
                    return Encoding.UTF8.GetString(transforme.TransformFinalBlock(dados, 0, dados.Length));
                }
            }
        }
    }
}
