using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class Utilidades
    {
        public static string GenerarSalt(int tamano = 16)
        {
            var saltBytes = new byte[tamano];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashearContrasena(string contrasena, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var contrasenaConSalt = contrasena + salt;
                byte[] contrasenaBytes = Encoding.UTF8.GetBytes(contrasenaConSalt);
                byte[] hashBytes = sha256.ComputeHash(contrasenaBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerificarContrasena(string contrasena, dynamic usuario)
        {
            if (usuario == null) return false;

            var hashContrasena = HashearContrasena(contrasena, usuario.salt);
            return hashContrasena == usuario.contrasenia;
        }

        public static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            // Convertir a minúsculas
            texto = texto.ToLower();

            // Reemplazar abreviaturas comunes
            texto = texto.Replace("col.", "colonia")
                         .Replace("av.", "avenida")
                         .Replace("cd.", "ciudad")
                         .Replace("cp.", "código postal");

            // Eliminar caracteres especiales
            texto = new string(texto.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

            return texto;
        }
    }
}
