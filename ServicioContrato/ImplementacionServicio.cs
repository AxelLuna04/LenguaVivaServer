using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.DAO;

namespace ServicioContrato
{
    public partial class ImplementacionServicio : IGestionUsuarios
    {
        public bool IniciarSesion(string usuario, string contrasenia)
        {
            // Simulación de base de datos
            Dictionary<string, string> usuarios = new Dictionary<string, string>
            {
                { "Axel", "12345" },
                { "Juan", "qwerty" },
                { "Maria", "abc123" }
            };

            return usuarios.ContainsKey(usuario) && usuarios[usuario] == contrasenia;

        }

		public bool RegistrarAdministrador(string nombreUsuario, string contrasenia, string nombre, string apellidos, string direccion, string email)
		{
            if (nombreUsuario != null || contrasenia != null || nombre != null || apellidos != null || direccion != null || email != null)
            {
                Console.WriteLine(nombreUsuario, contrasenia, nombre, apellidos, direccion, email);
                AccesoDatos.DAO.UsuarioDAO.AgregarAdministrador(nombreUsuario, contrasenia, nombre, apellidos, direccion, email);
                return true;
            }
            else
            {
                return false;
            }
        }
	}
}
    

