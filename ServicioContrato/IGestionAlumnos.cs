using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
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
        [OperationContract]
        bool RegistrarPago(int cantidadPagada, int idInscripcion);

        [OperationContract]
        ReciboPago ObtenerReciboPago(int idInscripcion);

        [OperationContract]
        bool EnviarCorreoReciboPago(int idInscripcion, string pdfPath);

        [OperationContract]
        CursoAlumno[] ObtenerCursosDeAlumno(int idAlumno);

        [OperationContract]
        int ObtenerIDInscripcionPorAlumnoYCurso(int idAlumno, int idCurso);

    }

    [DataContract]
    public class ReciboPago
    {
        [DataMember]
        public string curso { get; set; }

        [DataMember]
        public string alumno { get; set; }

        [DataMember]
        public string fechaPago { get; set; }

        [DataMember]
        public string cantidadPagada { get; set; }
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
