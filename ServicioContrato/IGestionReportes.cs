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
    public interface IGestionReportes
    {
        [OperationContract]
        CursoHorario[] ObtenerHorariosCursos(string correo);

        [OperationContract]
        ProfesorReporteDTO[] ObtenerReporteProfesoresActivos();

        [OperationContract]
        AlumnoReporteDTO[] ObtenerReporteAlumnosPorCurso(int idCurso);
    }

    [DataContract]
    public class CursoHorario
    {
        [DataMember]
        public string nombreCurso { get; set; }

        [DataMember]
        public TimeSpan horaInicio { get; set; }

        [DataMember]

        public TimeSpan horaSalida { get; set; }

        [DataMember]
        public string dia { get; set; }
    }

    [DataContract]
    public class ProfesorReporteDTO
    {
        [DataMember]
        public string NombreCompleto { get; set; }

        [DataMember]
        public int CursosActivos { get; set; }
    }

    [DataContract]
    public class AlumnoReporteDTO
    {
        [DataMember]
        public string nombreCompleto { get; set; }

        [DataMember]
        public string correo { get; set; }

        [DataMember]
        public string fechaInscripcion { get; set; }
    }
}
