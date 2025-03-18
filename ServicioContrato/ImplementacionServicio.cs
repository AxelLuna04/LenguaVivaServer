using AccesoDatos;
using AccesoDatos.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioContrato
{
    [ServiceBehavior]
    public class ImplementacionServicio : IGestionUsuarios, IGestionCursos
    {
        public bool IniciarSesion(string usuario, string contrasenia)
        {
            // Simulación de base de datos
            Dictionary<string, string> usuarios = new Dictionary<string, string>
            {
                { "Axel", "12345" },
                { "Juan", "qwerty" },
                { "Maria", "abc123" }
            };

            return usuarios.ContainsKey(usuario) && usuarios[usuario] == contrasenia;

        }

        //Metodos IGestionUsuarios
        public bool RegistrarAdministrador(AdministradorDTO administradorDTO)
        {
            try
            {
                Administrador administrador = new Administrador
                {
                    nombreUsuario = administradorDTO.nombreUsuario,
                    contrasenia = administradorDTO.contrasenia,
                    nombre = administradorDTO.nombre,
                    apellidos = administradorDTO.apellidos,
                    direccion = administradorDTO.direccion,
                    email = administradorDTO.email
                };

                UsuarioDAO.CrearAdministrador(administrador);
                return true;
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarProfesor(ProfesorDTO profesorDTO, int[] idIdiomas)
        {
            try
            {
                Profesor profesor = new Profesor
                {
                    nombreUsuario = profesorDTO.nombreUsuario,
                    contrasenia = profesorDTO.contrasenia,
                    nombre = profesorDTO.nombre,
                    apellidos = profesorDTO.apellidos,
                    direccion = profesorDTO.direccion,
                    email = profesorDTO.email
                };

                UsuarioDAO.CrearProfesor(profesor);

                foreach (var idIdioma in idIdiomas)
                {
                    Idioma_Profesor relacion = new Idioma_Profesor
                    {
                        idProfesor = profesor.idProfesor,
                        idIdioma = idIdioma
                    };
                    UsuarioDAO.CrearRelacion(relacion);
                }

                return true;
            }
            catch (EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }

        public IdiomaDTO[] CargarIdiomas()
        {
            try
            {
                var idiomas = IdiomaDAO.CargarIdiomas();
                if (idiomas != null)
                {
                    return idiomas.Select(i => new IdiomaDTO
                    {
                        idIdioma = i.idIdioma,
                        nombreIdioma = i.nombreIdioma
                    }).ToArray();
                }
                else
                {
                    Console.WriteLine("No se encontraron idiomas.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener idiomas: {ex.Message}");
                return null;
            }
        }

        public bool RegistrarAlumno(AlumnoDTO alumnoDTO, IdiomaNivelDTO[] idiomasNivel)
        {
            try
            {
                Alumno alumno = new Alumno
                {
                    nombre = alumnoDTO.nombre,
                    apellidos = alumnoDTO.apellidos,
                    direccion = alumnoDTO.direccion,
                    email = alumnoDTO.email,
                };

                UsuarioDAO.CrearAlumno(alumno);

                foreach (var idiomaNivel in idiomasNivel)
                {
                    Idioma_Alumno relacion = new Idioma_Alumno
                    {
                        idAlumno = alumno.idAlumno,
                        idIdioma = idiomaNivel.idIdioma,
                        nivel = idiomaNivel.nivel 
                    };
                    UsuarioDAO.CrearRelacionIdiomaAlumno(relacion);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        //Metodos IGestionUsuarios





        public CursoDTO ObtenerCursoPorID(int idCurso)
        {
            try
            {
                Curso curso = CursoDAO.ObtenerCursoPorID(idCurso);
                if (curso != null)
                {
                    var cursoDTO = new CursoDTO
                    {
                        idCurso = curso.idCurso,
                        nombreCurso = curso.nombreCurso,
                        descripcion = curso.descripcion,
                        nivel = (int)curso.nivel,
                        cupoMaximo = (int)curso.cupoMaximo,
                        estado = curso.estado,
                        fechaInicio = (DateTime)curso.fechaInicio,
                        fechaTermino = (DateTime)curso.fechaTermino,
                        listaAsistencia = curso.listaAsistencia,
                        idIdioma = curso.idIdioma,
                        idProfesor = curso.idProfesor
                    };
                    return cursoDTO;

                }
                else
                {
                    Console.WriteLine("No se encontró el curso con id: " + curso.idCurso);
                    return null;
                }

            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        // Metodos de IGestionCursos

        public Curso[] ObtenerCursos(string nombre)
        {
            try
            {
                Curso[] cursos = CursoDAO.ObtenerCursos();
                if (cursos != null)
                {
                    return cursos;
                }
                else
                {
                    Console.WriteLine("No se encontraron cursos");
                    return null;
                }

            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public CursoDTO[] ObtenerCursosPorNombre(string nombre)
        {
            try
            {
                Curso[] cursos = CursoDAO.ObtenerCursosPorNombre(nombre);
                if (cursos != null)
                {
                    var cursosDTO = cursos.Select(c => new CursoDTO
                    {
                        idCurso = c.idCurso,
                        nombreCurso = c.nombreCurso,
                        descripcion = c.descripcion,
                        nivel = (int)c.nivel,
                        cupoMaximo = (int)c.cupoMaximo,
                        estado = c.estado,
                        fechaInicio = (DateTime)c.fechaInicio,
                        fechaTermino = (DateTime)c.fechaTermino,
                        listaAsistencia = c.listaAsistencia,
                        idIdioma = c.idIdioma,
                        idProfesor = c.idProfesor
                    }).ToArray();
                    return cursosDTO;
                }
                else
                {
                    Console.WriteLine("No se encontraron cursos");
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public IdiomaDTO ObtenerIdiomaPorID(int idIdioma)
        {
            try
            {
                Idioma idioma = IdiomaDAO.ObtenerIdiomaPorID(idIdioma);
                if (idioma != null)
                {
                    var idiomaDTO = new IdiomaDTO
                    {
                        idIdioma = idioma.idIdioma,
                        nombreIdioma = idioma.nombreIdioma
                    };
                    return idiomaDTO;
                }
                else
                {
                    Console.WriteLine("No se encontró el idioma con id: " + idIdioma);
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public IdiomaDTO[] ObtenerIdiomas()
        {
            try
            {
                Idioma[] idiomas = IdiomaDAO.ObtenerIdiomas();
                if (idiomas != null)
                {
                    var idiomasDTO = idiomas.Select(i => new IdiomaDTO
                    {
                        idIdioma = i.idIdioma,
                        nombreIdioma = i.nombreIdioma
                    }).ToArray();

                    Console.WriteLine(idiomasDTO[0].nombreIdioma);
                    return idiomasDTO;
                }
                else
                {
                    Console.WriteLine("No se encontraron idiomas");
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public ProfesorDTO[] ObtenerProfesoresPorNombre(string nombre)
        {
            try
            {
                Profesor[] profesores = ProfesorDAO.ObtenerProfesoresPorNombre(nombre);
                if (profesores != null)
                {
                    var profesoresDTO = profesores.Select(p => new ProfesorDTO
                    {
                        idProfesor = p.idProfesor,
                        nombreUsuario = p.nombreUsuario,
                        nombre = p.nombre,
                        apellidos = p.apellidos,
                        direccion = p.direccion,
                        email = p.email
                    }).ToArray();
                    return profesoresDTO;
                }
                else
                {
                    Console.WriteLine("No se encontraron profesores");
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public ProfesorDTO ObtenerProfesorPorID(int idProfesor)
        {
            try
            {
                Profesor profesor = ProfesorDAO.ObtenerProfesorPorId(idProfesor);
                if (profesor != null)
                {
                    ProfesorDTO profesorDTO = new ProfesorDTO
                    {
                        idProfesor = profesor.idProfesor,
                        nombreUsuario = profesor.nombreUsuario,
                        nombre = profesor.nombre,
                        apellidos = profesor.apellidos,
                        direccion = profesor.direccion,
                        email = profesor.email
                    };
                    return profesorDTO;
                }
                else
                {
                    Console.WriteLine("No se encontró el profesor con id: " + idProfesor);
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }
        }

        public bool RegistrarCurso(CursoDTO cursoDTO)
        {
            try
            {
                Curso curso = new Curso
                {
                    nombreCurso = cursoDTO.nombreCurso,
                    descripcion = cursoDTO.descripcion,
                    nivel = cursoDTO.nivel,
                    cupoMaximo = cursoDTO.cupoMaximo,
                    estado = "Propuesto",//TODO: Convertir en Constante ?
                    fechaInicio = cursoDTO.fechaInicio,
                    fechaTermino = cursoDTO.fechaTermino,
                    idIdioma = cursoDTO.idIdioma,
                    idProfesor = cursoDTO.idProfesor
                };

                CursoDAO.CrearCurso(curso);
                return true;
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }

        public bool ModificarCurso(CursoDTO cursoDTO)
        {
            try
            {
                Curso curso = CursoDAO.ObtenerCursoPorID(cursoDTO.idCurso);
                if (curso != null)
                {
                    curso.nombreCurso = cursoDTO.nombreCurso;
                    curso.descripcion = cursoDTO.descripcion;
                    curso.nivel = cursoDTO.nivel;
                    curso.cupoMaximo = cursoDTO.cupoMaximo;
                    curso.estado = cursoDTO.estado;
                    curso.fechaInicio = cursoDTO.fechaInicio;
                    curso.fechaTermino = cursoDTO.fechaTermino;
                    curso.idIdioma = cursoDTO.idIdioma;
                    curso.idProfesor = cursoDTO.idProfesor;

                    CursoDAO.ModificarCurso(curso);
                    return true;
                }
                else
                {
                    Console.WriteLine("No se encontró el curso con id: " + cursoDTO.idCurso);
                    return false;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }

        public CursoDTO[] ObtenerCursosPorNombreIdioma(string nombreIdioma)
        {
            try
            {
                Curso[] cursos = CursoDAO.ObtenerCursosPorNombreIdioma(nombreIdioma);
                if (cursos != null)
                {
                    var cursosDTO = cursos.Select(c => new CursoDTO
                    {
                        idCurso = c.idCurso,
                        nombreCurso = c.nombreCurso,
                        descripcion = c.descripcion,
                        nivel = (int)c.nivel,
                        cupoMaximo = (int)c.cupoMaximo,
                        estado = c.estado,
                        fechaInicio = (DateTime)c.fechaInicio,
                        fechaTermino = (DateTime)c.fechaTermino,
                        listaAsistencia = c.listaAsistencia,
                        idIdioma = c.idIdioma,
                        idProfesor = c.idProfesor
                    }).ToArray();
                    return cursosDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }

        }

        public CursoDTO[] ObtenerCursosPorNivel(int nivel)
        {
            try
            {
                Curso[] cursos = CursoDAO.ObtenerCursosPorNivel(nivel);
                if (cursos != null)
                {
                    var cursosDTO = cursos.Select(c => new CursoDTO
                    {
                        idCurso = c.idCurso,
                        nombreCurso = c.nombreCurso,
                        descripcion = c.descripcion,
                        nivel = (int)c.nivel,
                        cupoMaximo = (int)c.cupoMaximo,
                        estado = c.estado,
                        fechaInicio = (DateTime)c.fechaInicio,
                        fechaTermino = (DateTime)c.fechaTermino,
                        listaAsistencia = c.listaAsistencia,
                        idIdioma = c.idIdioma,
                        idProfesor = c.idProfesor
                    }).ToArray();
                    return cursosDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }

        }

        public CursoDTO[] ObtenerCursosPorNombreProfesor(string nombreProfesor)
        {
            try
            {
                Curso[] cursos = CursoDAO.ObtenerCursosPorNombreProfesor(nombreProfesor);
                if (cursos != null)
                {
                    var cursosDTO = cursos.Select(c => new CursoDTO
                    {
                        idCurso = c.idCurso,
                        nombreCurso = c.nombreCurso,
                        descripcion = c.descripcion,
                        nivel = (int)c.nivel,
                        cupoMaximo = (int)c.cupoMaximo,
                        estado = c.estado,
                        fechaInicio = (DateTime)c.fechaInicio,
                        fechaTermino = (DateTime)c.fechaTermino,
                        listaAsistencia = c.listaAsistencia,
                        idIdioma = c.idIdioma,
                        idProfesor = c.idProfesor
                    }).ToArray();
                    return cursosDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return null;
            }

        }
    }
}
