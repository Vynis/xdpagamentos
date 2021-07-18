using System;

public static class HelperCriptografiaTDES
{
    private static byte[] _chave = Encoding.UTF8.GetBytes(SettingsToken.Secret);

    /// <summary>
    /// Método para realizar a criptografia usando o algorítmo TriploDes
    /// </summary>
    /// <param name="valor"> Valor que será criptografado</param>
    /// <returns>Valor criptografado</returns>
    public static string Criptografar(this string valor)
    {
        using (var hashprovider = new MD5CryptoServiceProvider())
        {
            var encriptar = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Key = hashprovider.ComputeHash(_chave),
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
    public static string Descriptar(this string valor)
    {
        using (var hashProvider = new MD5CryptoServiceProvider())
        {
            var descriptografar = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Key = hashProvider.ComputeHash(_chave),
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
