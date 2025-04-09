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
    public interface IGestionAlumnos
    {
        [OperationContract]
        bool RegistrarInscripcion(InscripcionDTO inscripcionDTO);
        [OperationContract]
        AlumnoDTO[] BuscarAlumnos(string criterioBusqueda);
        [OperationContract]
        int ObtenerNumInscripciones(int idCurso);
        [OperationContract]
        bool InscripcionExistente(int idCurso, int idAlumno);

    }
    [DataContract]
    public class InscripcionDTO
    {
        [DataMember]
        public int idInscripcion { get; set; }
        [DataMember]
        public int idCurso { get; set; }
        [DataMember]
        public int idAlumno { get; set; }
        [DataMember]
        public DateTime fechaInscripcion { get; set; }
    }
}
