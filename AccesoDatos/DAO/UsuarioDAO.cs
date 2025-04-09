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
                var profesor = contexto.Profesor.Include(p => p.Idioma_Profesor).FirstOrDefault(p => p.email == correo);
                return profesor;
            }
        }

        public Alumno ObtenerAlumnoPorCorreo(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Alumno.FirstOrDefault(a => a.email == correo);
            }
        }

        public void EliminarAdministrador(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var admin = contexto.Administrador.FirstOrDefault(a => a.email == correo);
                if (admin != null)
                {
                    contexto.Administrador.Remove(admin);
                    contexto.SaveChanges();
                }
            }
        }

        public void EliminarProfesor(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var profesor = contexto.Profesor.FirstOrDefault(p => p.email == correo);
                if (profesor != null)
                {
                    var relaciones = contexto.Idioma_Profesor.Where(r => r.idProfesor == profesor.idProfesor).ToList();
                    contexto.Idioma_Profesor.RemoveRange(relaciones);

                    contexto.Profesor.Remove(profesor);

                    contexto.SaveChanges();
                }
            }
        }

        public void EliminarAlumno(string correo)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var alumno = contexto.Alumno.FirstOrDefault(a => a.email == correo);
                if (alumno != null)
                {
                    var relaciones = contexto.Idioma_Alumno.Where(r => r.idAlumno == alumno.idAlumno).ToList();
                    contexto.Idioma_Alumno.RemoveRange(relaciones);

                    contexto.Alumno.Remove(alumno);

                    contexto.SaveChanges();
                }
            }
        }

        public Alumno ObtenerAlumnoPorID(int idAlumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Alumno.FirstOrDefault(a => a.idAlumno == idAlumno);
            }
        }

        public void ModificarAdministrador(Administrador administradorExistente)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Entry(administradorExistente).State = EntityState.Modified;
                contexto.SaveChanges();
            }
        }

        public void ModificarProfesorCompleto(Profesor profesorModificado, List<int> idiomasSeleccionados)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Entry(profesorModificado).State = EntityState.Modified;

                
                var relacionesActuales = contexto.Idioma_Profesor
                    .Where(ip => ip.idProfesor == profesorModificado.idProfesor)
                    .ToList();

                var idsActuales = relacionesActuales.Select(ip => ip.idIdioma).ToList();
                var idsNuevos = idiomasSeleccionados ?? new List<int>();

                var aEliminar = relacionesActuales
                    .Where(ip => !idsNuevos.Contains(ip.idIdioma))
                    .ToList();

                var aAgregar = idsNuevos
                    .Where(id => !idsActuales.Contains(id))
                    .ToList();

                if (aEliminar.Any())
                    contexto.Idioma_Profesor.RemoveRange(aEliminar);

                foreach (var idIdioma in aAgregar)
                {
                    contexto.Idioma_Profesor.Add(new Idioma_Profesor
                    {
                        idProfesor = profesorModificado.idProfesor,
                        idIdioma = idIdioma
                    });
                }

                // 5) Guardar TODO junto
                contexto.SaveChanges();
            }
        }

        public Profesor ObtenerProfesorPorId(int id)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Profesor.FirstOrDefault(p => p.idProfesor == id);
            }
        }

        public void ModificarAlumnoCompleto(Alumno alumnoModificado, List<Idioma_Alumno> nuevosIdiomas)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Entry(alumnoModificado).State = EntityState.Modified;

                var relacionesActuales = contexto.Idioma_Alumno
                    .Where(ia => ia.idAlumno == alumnoModificado.idAlumno)
                    .ToList();

                foreach (var nuevo in nuevosIdiomas)
                {
                    var existente = relacionesActuales.FirstOrDefault(ra => ra.idIdioma == nuevo.idIdioma);
                    if (existente != null)
                    {
                        existente.nivel = nuevo.nivel;
                        contexto.Entry(existente).State = EntityState.Modified;
                    }
                    else
                    {
                        contexto.Idioma_Alumno.Add(nuevo);
                    }
                }

                foreach (var actual in relacionesActuales)
                {
                    if (!nuevosIdiomas.Any(n => n.idIdioma == actual.idIdioma))
                    {
                        contexto.Idioma_Alumno.Remove(actual);
                    }
                }

                contexto.SaveChanges();
            }
        }

        public Alumno ObtenerAlumnoPorId(int idAlumno)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Alumno.FirstOrDefault(a => a.idAlumno == idAlumno);
            }
        }

        public Alumno[] ObtenerAlumnosPorNombre(string nombre)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Alumno
                    .Where(a => a.nombre.Contains(nombre) ||
                    a.apellidos.Contains(nombre))
                    .ToArray();
            }
        }
    }
}