using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Interfaces;

namespace AccesoDatos.DAO
{
    public class CursoDAO : ICursoDAO
    {
        public   void CrearCurso(Curso curso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                contexto.Curso.Add(curso);
                contexto.SaveChanges();
            }
        }

        public   void ModificarCurso(Curso curso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                contexto.Entry(curso).State = EntityState.Modified;
                contexto.SaveChanges();
            }
        }

        public   void EliminarCurso(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                contexto.Curso
                    .Remove(contexto.Curso.Find(idCurso));
                
                contexto.SaveChanges();
            }
        }

        public   Curso[] ObtenerCursos()
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso.ToArray();
            }

        }

        public   Curso[] ObtenerCursosPorNombre(String nombreCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso
                    .Where(c => c.nombreCurso.Contains(nombreCurso))
                    .ToArray();
            }
        }

        public   Curso ObtenerCursoPorID(int id)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso.Find(id);
            }
        }

        public   Curso[] ObtenerCursosPorNombreIdioma(String nombreIdioma)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso
                    .Where(c => c.Idioma.nombreIdioma.Contains(nombreIdioma))
                    .ToArray();
            }
        }

        public   Curso[] ObtenerCursosPorNombreProfesor(String nombreProfesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso
                    .Where(c => c.Profesor.nombre.Contains(nombreProfesor) ||
                    c.Profesor.apellidos.Contains(nombreProfesor))
                    .ToArray();
            }
        }

        public   Curso[] ObtenerCursosPorNivel(int nivel)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Curso
                    .Where(c => c.nivel == nivel)
                    .ToArray();
            }
        }

        public bool TieneAlumnosInscritos(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                if (contexto.Inscripcion
                    .Any(ec => ec.idCurso == idCurso))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SubirListaAlumnos(int idCurso, byte[] archivo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var curso = contexto.Curso.Find(idCurso);
                if (curso == null)
                {
                    return false;
                }
                else
                {
                    curso.listaAsistencia = archivo;
                    contexto.SaveChanges();
                    return true;
                }
                
            }
        }

        public Curso[] ObtenerCursosPorIdProfesor(int idProfesor)
        {
            using(var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Curso
                    .Where(c => c.idProfesor  == idProfesor)
                    .ToArray();
            }
        }


    }
}
