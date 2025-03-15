using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioContrato
{
    [ServiceContract]
    public interface IGestionUsuarios
    {
        [OperationContract]
        bool IniciarSesion(string usuario, string contrasenia);

        [OperationContract]
        bool RegistrarAdministrador(string nombreUsuario, string contrasenia, string nombre, string apellidos, string direccion, string email);


    }



	public class Administrador
	{
		public int Id { get; set; } 
		public string NombreUsuario { get; set; } 
		public string Contraseña { get; set; }
		public string Nombre { get; set; } 
		public string Apellidos { get; set; } 
		public string Direccion { get; set; } 
		public string Correo { get; set; } 
	}

}
