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
        bool RegistrarPago(int cantidadPagada, int idInscripcion);

        [OperationContract]
        ReciboPago ObtenerReciboPago(int idInscripcion);

        [OperationContract]
        bool EnviarCorreoReciboPago(int idInscripcion, string pdfPath);
        
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
}
