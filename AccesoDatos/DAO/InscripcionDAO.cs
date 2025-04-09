using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class InscripcionDAO
    {
        public int ObtenerIDCurso(int idInscripcion)
        {
            int idCurso;

            using (var contexto = new LenguaVivaDBEntities())
            {
                idCurso = contexto.Inscripcion.FirstOrDefault(i => i.idInscripcion == idInscripcion).idCurso;
                return idCurso;
            }
        }

        public int ObtenerIDAlumno(int idInscripcion)
        {
            int idAlumno;

            using (var contexto = new LenguaVivaDBEntities())
            {
                idAlumno = contexto.Inscripcion.FirstOrDefault(i => i.idInscripcion == idInscripcion).idAlumno;
                return idAlumno;
            }
        }

        public Curso[] ObtenerCursosPorAlumno(int idAlumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;

                var cursos = contexto.Inscripcion
                    .Where(i => i.idAlumno == idAlumno)
                    .Select(i => i.Curso)
                    .ToArray();

                return cursos;
            }
        }

        public Inscripcion ObtenerInscripcionPorCursoYAlumno(int idAlumno, int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;

                var inscripcion = contexto.Inscripcion.FirstOrDefault(i => i.idAlumno == idAlumno && i.idCurso == idCurso);
                return inscripcion;
            }
        }

    }
}
