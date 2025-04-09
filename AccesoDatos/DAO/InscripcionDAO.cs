using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Interfaces;

namespace AccesoDatos.DAO
{
    public class InscripcionDAO : IInscripcionDAO
    {
        public void RegistrarInscripcion(Inscripcion inscripcion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Inscripcion.Add(inscripcion);
                contexto.SaveChanges();
            }
        }

        public int ObtenerNumInscripciones(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                Inscripcion[] inscripciones = contexto.Inscripcion
                    .Where(i => i.idCurso == idCurso)
                    .ToArray();

                return inscripciones.Length;
            }
        }
        public bool InscripcionExistente(int idCurso, int idAlumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                Inscripcion inscripcion = contexto.Inscripcion
                    .FirstOrDefault(i => i.idCurso == idCurso && i.idAlumno == idAlumno);

                return inscripcion != null;
            }
        }
    }
}
