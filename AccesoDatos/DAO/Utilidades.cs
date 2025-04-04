using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

        public static bool VerificarContrasenaAdministrador(string contrasena, Administrador administrador)
        {
            var hashContrasena = HashearContrasena(contrasena, administrador.salt);
            if (hashContrasena == administrador.contrasenia)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool VerificarContrasenaProfesor(string contrasena, Profesor profesor)
        {
            var hashContrasena = HashearContrasena(contrasena, profesor.salt);
            if (hashContrasena == profesor.contrasenia)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;
            texto = texto.ToLower();

            texto = texto.Replace("col.", "colonia")
                         .Replace("av.", "avenida")
                         .Replace("cd.", "ciudad")
                         .Replace("cp.", "código postal");

            texto = new string(texto.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

            return texto;
        }

        public static void EnviarCorreo(string correoAlumno, string pdfPath)
        {
            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("hunterstrophy01@gmail.com");
                correo.To.Add(correoAlumno);
                correo.Subject = "Recibo de Pago - Lengua Viva";
                correo.Body = "Adjunto encontrarás el recibo de pago correspondiente.";
                correo.Attachments.Add(new Attachment(pdfPath));
                correo.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("hunterstrophy01@gmail.com", "azcx qzqh kzdq ifve");
                smtp.EnableSsl = true;
                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}
