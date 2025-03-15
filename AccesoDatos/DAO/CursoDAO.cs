using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class CursoDAO
    {
        public static void CrearCurso(Curso curso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Curso.Add(curso);
                contexto.SaveChanges();
            }
        }

        public static void ModificarCurso(Curso curso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Entry(curso).State = EntityState.Modified;
                contexto.SaveChanges();
            }
        }

        public static void EliminarCurso(Curso curso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Curso.Remove(curso);
                contexto.SaveChanges();
            }
        }

        public static Curso[] ObtenerCursos()
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Curso.ToArray();
            }

        }

        public static Curso ObtenerCursoPorNombre(String nombreCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Curso.FirstOrDefault(c => c.nombreCurso == nombreCurso);
            }
        }

    }
}
