using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Interfaces;

namespace AccesoDatos.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {
        public void CrearAdministrador(Administrador administrador)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Administrador.Add(administrador);
                contexto.SaveChanges();
            }
        }
        public void CrearProfesor(Profesor profesor)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Profesor.Add(profesor);
                contexto.SaveChanges();
            }
        }
        public void CrearRelacion(Idioma_Profesor relacion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Idioma_Profesor.Add(relacion);
                contexto.SaveChanges();
            }
        }

        public void CrearAlumno(Alumno alumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Alumno.Add(alumno);
                contexto.SaveChanges();
            }
        }

        public void CrearRelacionIdiomaAlumno(Idioma_Alumno relacion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Idioma_Alumno.Add(relacion);
                contexto.SaveChanges();
            }
        }

        public int ObtenerTipoUsuario(string correo)
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

        public Administrador ObtenerAdministradorPorCorreo(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var admin = contexto.Administrador.FirstOrDefault(a => a.email == correo);
                return admin;
            }
        }

        public Profesor ObtenerProfesorPorCorreo(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var profesor = contexto.Profesor.FirstOrDefault(p => p.email == correo);
                return profesor;
            }
        }
    }
}