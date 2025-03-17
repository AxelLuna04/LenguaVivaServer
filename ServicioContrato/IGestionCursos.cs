using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ServicioContrato
{
    [ServiceContract]
    public interface IGestionCursos
    {
        [OperationContract]
        Curso[] ObtenerCursos(String nombre);

        [OperationContract]
        bool RegistrarCurso(CursoDTO cursoDTO);

        [OperationContract]
        ProfesorDTO[] ObtenerProfesoresPorNombre(String nombre);

        [OperationContract]
        IdiomaDTO[] ObtenerIdiomas();


    }
    [DataContract]
    public class CursoDTO
    {
        [DataMember]
        public int idCurso { get; set; }

        [DataMember]
        public string nombreCurso { get; set; }

        [DataMember]
        public string descripcion { get; set; }

        [DataMember]
        public int nivel { get; set; }

        [DataMember]
        public int cupoMaximo { get; set; }

        [DataMember]
        public string estado { get; set; }

        [DataMember]
        public DateTime fechaInicio { get; set; }

        [DataMember]
        public DateTime fechaTermino { get; set; }

        [DataMember]
        public byte[] listaAsistencia { get; set; }

        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public int idProfesor { get; set; }
    }

    [DataContract]
    public class ProfesorDTO
    {
        [DataMember]
        public int idProfesor { get; set; }

        [DataMember]
        public string nombreUsuario { get; set; }

        [DataMember]
        public string nombre { get; set; }

        [DataMember]
        public string apellidos { get; set; }

        [DataMember]
        public string direccion { get; set; }

        [DataMember]
        public string email { get; set; }
         
    }


    [DataContract]
    public class IdiomaDTO
    {
        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public string nombreIdioma { get; set; }
    }
}
