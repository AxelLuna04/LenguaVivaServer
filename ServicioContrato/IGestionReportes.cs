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
        public string dia {  get; set; }
    }
}
