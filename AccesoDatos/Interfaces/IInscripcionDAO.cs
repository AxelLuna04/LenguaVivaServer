using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaces
{
    public interface IInscripcionDAO
    {
        void RegistrarInscripcion(Inscripcion inscripcion);
        int ObtenerNumInscripciones(int idCurso);
        bool InscripcionExistente(int idCurso, int idAlumno);
        Inscripcion[] ObtenerInscripcionesPorIDCurso(int idCurso);
    }
}
