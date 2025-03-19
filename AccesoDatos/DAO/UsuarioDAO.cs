using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class UsuarioDAO
    {
        public static void CrearAdministrador(Administrador administrador)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Administrador.Add(administrador);
                contexto.SaveChanges();
            }
        }
        public static void CrearProfesor(Profesor profesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Profesor.Add(profesor);
                contexto.SaveChanges();
            }
        }
        public static void CrearRelacion(Idioma_Profesor relacion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Idioma_Profesor.Add(relacion);
                contexto.SaveChanges();
            }
        }

        public static void CrearAlumno(Alumno alumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Alumno.Add(alumno);
                contexto.SaveChanges();
            }
        }

        public static void CrearRelacionIdiomaAlumno(Idioma_Alumno relacion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Idioma_Alumno.Add(relacion);
                contexto.SaveChanges();
            }
        }

        public static int ObtenerTipoUsuario(string correo)
        {
            int resultado = 0;

            using (var contexto = new LenguaVivaDBEntities())
            {
                // Buscar si es Administrador
                var admin = contexto.Administrador.FirstOrDefault(a => a.email == correo);
                if (admin != null)
                {
                    resultado = 1;
                }

                // Buscar si es Profesor
                var profesor = contexto.Profesor.FirstOrDefault(p => p.email == correo);
                if (profesor != null)
                {
                    resultado = 2;
                }
            }

            return resultado;
        }

        public static Administrador ObtenerAdministradorPorCorreo(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var admin = contexto.Administrador.FirstOrDefault(a => a.email == correo);
                return contexto.Administrador.Find(correo);
            }
        }

        public static Profesor ObtenerProfesorPorCorreo(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var profesor = contexto.Profesor.FirstOrDefault(p => p.email == correo);
                return profesor;
            }
        }
    }
}