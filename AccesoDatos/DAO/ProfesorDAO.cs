using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Interfaces;

namespace AccesoDatos.DAO
{
    public class ProfesorDAO :IProfesorDAO
    {
        public void CrearProfesor(Profesor profesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Profesor.Add(profesor);
                contexto.SaveChanges();
            }
        }

        public void ModificarProfesor(Profesor profesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Entry(profesor).State = EntityState.Modified;
                contexto.SaveChanges();
            }
        }

        public void EliminarProfesor(Profesor profesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Profesor.Remove(profesor);
                contexto.SaveChanges();
            }
        }

        public Profesor[] ObtenerProfesores()
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Profesor.ToArray();
            }

        }

        public Profesor ObtenerProfesorPorId(int id)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Profesor.Find(id);
            }
        }

        public Profesor ObtenerProfesorPorUsuario(String usuario)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Profesor.FirstOrDefault(p => p.nombreUsuario == usuario);
            }
        }

        public Profesor[] ObtenerProfesoresPorNombre(string nombre)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Profesor
                    .Where(p => p.nombre.Contains(nombre) ||
                    p.apellidos.Contains(nombre))
                    .ToArray();
            }
        }

    }
}