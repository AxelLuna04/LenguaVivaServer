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
        SesionUsuario IniciarSesion(string usuario, string contrasenia);

        [OperationContract]
        bool RegistrarAdministrador(AdministradorDTO administradorDTO);

        [OperationContract]
        IdiomaDTO[] CargarIdiomas();

        [OperationContract]
        bool RegistrarProfesor(ProfesorDTO profesorDTO, int[] idIdiomas);
        
        [OperationContract] 
        bool RegistrarAlumno(AlumnoDTO alumnoDTO, IdiomaNivelDTO[] idiomasNivel);

        [OperationContract]
        UsuarioDTO[] BuscarUsuarios(string criterioBusqueda, int tipoUsuario);

        [OperationContract]
        UsuarioDTO EliminarUsuario(string correo, int tipoUsuario);

        [OperationContract]
        bool ModificarAdministrador(AdministradorDTO administradorDTO);

        [OperationContract]
        AdministradorDTO ObtenerAdministradorPorCorreo(string correo);

        [OperationContract]
        ProfesorDTO ObtenerProfesorPorCorreo(string correo);

        [OperationContract]
        AlumnoDTO ObtenerAlumnoPorCorreo(string correo);

        [OperationContract]
        bool ModificarProfesor(ProfesorDTO profesorDTO);

        [OperationContract]
        bool ModificarAlumno(AlumnoDTO alumnoDTO, IdiomaNivelDTO[] idiomasNivel);

        [OperationContract]
        bool RegistrarIdioma(IdiomaDTO idiomaDTO, out string mensaje);
    }

    [DataContract]
    public class AdministradorDTO
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
        [DataMember]
        public string telefono { get; set; }
    }

    [DataContract]
    public class ProfesorDTO
    {
        [DataMember]
        public int idProfesor { get; set; }

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
        [DataMember]
        public string telefono { get; set; }

        [DataMember]
        public List<int> idiomasSeleccionados { get; set; }

    }

    [DataContract]
    public class AlumnoDTO
    {
        [DataMember]
        public int idAlumno { get; set; }

        [DataMember]
        public string nombre { get; set; }

        [DataMember]
        public string apellidos { get; set; }

        [DataMember]
        public string direccion { get; set; }

        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string telefono { get; set; }
        [DataMember]
        public IdiomaNivelDTO[] idiomasNivel { get; set; }


    }

    [DataContract]
    public class IdiomaNivelDTO
    {
        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public int nivel { get; set; } 

        [DataMember]
        public string idiomaNombre { get; set; }

    }

    [DataContract]
    public class IdiomaDTO
    {
        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public string nombreIdioma { get; set; }

        [DataMember]
        public bool IsSelected { get; set; }

    }

    [DataContract]
    public class SesionUsuario
    {
        [DataMember]
        public bool esValido { get; set; }

        [DataMember]
        public string correo { get; set; }

        [DataMember]
        public int tipoUsuario { get; set; }
    }

    [DataContract]
    public class UsuarioDTO
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember] 
        public int nombreUsuario { get; set; }

        [DataMember]
        public string apellidos { get; set; }

        [DataMember]
        public string correo { get; set; }

        [DataMember]
        public string telefono { get; set; }

        [DataMember]
        public string direccion { get; set; }

        [DataMember]
        public string nivelIdioma { get; set; }
        [DataMember]
        public int tipoUsuario { get; set; }

    }

}
