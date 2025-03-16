using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

    [DataContract]
    public class Administrador
    {
        [DataMember]
        public int idAdministrador { get; set; }
        [DataMember]
        public string nombreUsuario { get; set; }
        [DataMember]
        public string contrasenia { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string apellidos { get; set; }
        [DataMember]
        public string direccion { get; set; }
        [DataMember]
        public string email { get; set; }
    }
}
