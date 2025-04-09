using AccesoDatos;
using AccesoDatos.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Interfaces;

namespace ServicioContrato
{
    [ServiceBehavior]
    public class ImplementacionServicio : IGestionUsuarios, IGestionCursos, IGestionAlumnos
    {
        private readonly IIdiomaDAO idiomaDAO;
        private readonly ICursoDAO cursoDAO;
        private readonly IUsuarioDAO usuarioDAO;
        private readonly IProfesorDAO profesorDAO;
        private readonly IInscripcionDAO inscripcionDAO;
        private readonly IHorarioDAO horarioDAO;

        //Constructor por defecto para la implementacion real
        public ImplementacionServicio()
        {
            idiomaDAO = new IdiomaDAO();
            cursoDAO = new CursoDAO();
            usuarioDAO = new UsuarioDAO();
            profesorDAO = new ProfesorDAO();
            inscripcionDAO = new InscripcionDAO();
            horarioDAO = new HorarioDAO();
        }

        //Constructor para mockear las pruebas
        public ImplementacionServicio(IIdiomaDAO idiomaDAO, ICursoDAO cursoDAO, IUsuarioDAO usuarioDAO, IProfesorDAO profesorDAO, IInscripcionDAO inscripcionDAO, IHorarioDAO horarioDAO)
        {
            this.idiomaDAO = idiomaDAO;
            this.cursoDAO = cursoDAO;
            this.usuarioDAO = usuarioDAO;
            this.profesorDAO = profesorDAO;
            this.inscripcionDAO = inscripcionDAO;
            this.horarioDAO = horarioDAO;
        }

        public SesionUsuario IniciarSesion(string correo, string contrasenia)
        {
            SesionUsuario sesionUsuario = new SesionUsuario();
            sesionUsuario.esValido = false;

            if (usuarioDAO.ObtenerTipoUsuario(correo) == 1)
            {
                if (Utilidades.VerificarContrasenaAdministrador(contrasenia, usuarioDAO.ObtenerAdministradorPorCorreo(correo)))
                {
                    sesionUsuario.esValido = true;
                    sesionUsuario.tipoUsuario = 1;
                    sesionUsuario.correo = correo;
                }
            }

            if (usuarioDAO.ObtenerTipoUsuario(correo) == 2)
            {
                if (Utilidades.VerificarContrasenaProfesor(contrasenia, usuarioDAO.ObtenerProfesorPorCorreo(correo)))
                {
                    sesionUsuario.esValido = true;
                    sesionUsuario.tipoUsuario = 2;
                    sesionUsuario.correo = correo;
                }
            }
            return sesionUsuario;

        }

        //Metodos IGestionUsuarios
        public bool RegistrarAdministrador(AdministradorDTO administradorDTO)
        {
            string salt = Utilidades.GenerarSalt();
            try
            {
                Administrador administrador = new Administrador
                {
                    nombreUsuario = administradorDTO.nombreUsuario,
                    contrasenia = Utilidades.HashearContrasena(administradorDTO.contrasenia, salt),
                    salt = salt,
                    nombre = administradorDTO.nombre,
                    apellidos = administradorDTO.apellidos,
                    direccion = administradorDTO.direccion,
                    email = administradorDTO.email,
                    telefono = administradorDTO.telefono
                };

                usuarioDAO.CrearAdministrador(administrador);
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
            string salt = Utilidades.GenerarSalt();
            try
            {
                Profesor profesor = new Profesor
                {
                    nombreUsuario = profesorDTO.nombreUsuario,
                    contrasenia = Utilidades.HashearContrasena(profesorDTO.contrasenia, salt),
                    salt = salt,
                    nombre = profesorDTO.nombre,
                    apellidos = profesorDTO.apellidos,
                    direccion = profesorDTO.direccion,
                    email = profesorDTO.email,
                    telefono = profesorDTO.telefono
                };

                usuarioDAO.CrearProfesor(profesor);

                foreach (var idIdioma in idIdiomas)
                {
                    Idioma_Profesor relacion = new Idioma_Profesor
                    {
                        idProfesor = profesor.idProfesor,
                        idIdioma = idIdioma
                    };
                    usuarioDAO.CrearRelacion(relacion);
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
                var idiomas = idiomaDAO.CargarIdiomas();
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
                    telefono = alumnoDTO.telefono
                };

                usuarioDAO.CrearAlumno(alumno);

                foreach (var idiomaNivel in idiomasNivel)
                {
                    Idioma_Alumno relacion = new Idioma_Alumno
                    {
                        idAlumno = alumno.idAlumno,
                        idIdioma = idiomaNivel.idIdioma,
                        nivel = idiomaNivel.nivel
                    };
                    usuarioDAO.CrearRelacionIdiomaAlumno(relacion);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public UsuarioDTO[] BuscarUsuarios(string criterioBusqueda, int tipoUsuario)
        {
            Console.WriteLine(criterioBusqueda);
            try
            {
                using (var contexto = new LenguaVivaDBEntities())
                {
                    var resultados = new List<UsuarioDTO>();
                    string criterioNormalizado = criterioBusqueda.ToLower();

                    if (tipoUsuario == 2)
                    {
                        // Si el filtro indica 2 (por ejemplo, profesor buscando sus alumnos)
                        Console.WriteLine("Filtrando solo alumnos...");
                        var alumnos = contexto.Alumno
                            .Where(al => al.nombre.ToLower().Contains(criterioNormalizado) ||
                                         al.apellidos.ToLower().Contains(criterioNormalizado) ||
                                         al.direccion.ToLower().Contains(criterioNormalizado) ||
                                         al.email.ToLower().Contains(criterioNormalizado) ||
                                         al.telefono.Contains(criterioNormalizado) ||
                                         (contexto.Idioma_Alumno
                                            .Where(ia => ia.idAlumno == al.idAlumno)
                                            .Select(ia => ia.nivel.ToString())
                                            .FirstOrDefault())
                                            .Contains(criterioNormalizado))
                            .Select(al => new UsuarioDTO
                            {
                                nombre = al.nombre,
                                apellidos = al.apellidos,
                                direccion = al.direccion,
                                correo = al.email,
                                telefono = al.telefono,
                                nivelIdioma = (contexto.Idioma_Alumno
                                    .Where(ia => ia.idAlumno == al.idAlumno)
                                    .Select(ia => ia.nivel.ToString())
                                    .FirstOrDefault()),
                                tipoUsuario = 3  // Alumno
                            }).ToList();

                        resultados.AddRange(alumnos);
                    }
                    else
                    {
                        Console.WriteLine("Buscando administradores, profesores y alumnos...");

                        var administradores = contexto.Administrador
                            .Where(a => a.nombre.ToLower().Contains(criterioNormalizado) ||
                                        a.apellidos.ToLower().Contains(criterioNormalizado) ||
                                        a.direccion.ToLower().Contains(criterioNormalizado) ||
                                        a.telefono.Contains(criterioNormalizado) ||
                                        a.email.ToLower().Contains(criterioNormalizado))
                            .Select(a => new UsuarioDTO
                            {
                                nombre = a.nombre,
                                apellidos = a.apellidos,
                                direccion = a.direccion,
                                correo = a.email,
                                telefono = a.telefono,
                                nivelIdioma = null,
                                tipoUsuario = 1
                            }).ToList();

                        resultados.AddRange(administradores);

                        var profesores = contexto.Profesor
                            .Where(p => p.nombre.ToLower().Contains(criterioNormalizado) ||
                                        p.apellidos.ToLower().Contains(criterioNormalizado) ||
                                        p.direccion.ToLower().Contains(criterioNormalizado) ||
                                        p.telefono.Contains(criterioNormalizado) ||
                                        p.email.ToLower().Contains(criterioNormalizado))
                            .Select(p => new UsuarioDTO
                            {
                                nombre = p.nombre,
                                apellidos = p.apellidos,
                                direccion = p.direccion,
                                correo = p.email,
                                telefono = p.telefono,
                                nivelIdioma = null,
                                tipoUsuario = 2
                            }).ToList();

                        resultados.AddRange(profesores);

                        var alumnos = contexto.Alumno
                            .Where(al => al.nombre.ToLower().Contains(criterioNormalizado) ||
                                         al.apellidos.ToLower().Contains(criterioNormalizado) ||
                                         al.direccion.ToLower().Contains(criterioNormalizado) ||
                                         al.email.ToLower().Contains(criterioNormalizado) ||
                                         al.telefono.Contains(criterioNormalizado) ||
                                         (contexto.Idioma_Alumno
                                            .Where(ia => ia.idAlumno == al.idAlumno)
                                            .Select(ia => ia.nivel.ToString())
                                            .FirstOrDefault())
                                            .Contains(criterioNormalizado))
                            .Select(al => new UsuarioDTO
                            {
                                nombre = al.nombre,
                                apellidos = al.apellidos,
                                direccion = al.direccion,
                                correo = al.email,
                                telefono = al.telefono,
                                nivelIdioma = (contexto.Idioma_Alumno
                                    .Where(ia => ia.idAlumno == al.idAlumno)
                                    .Select(ia => ia.nivel.ToString())
                                    .FirstOrDefault()),
                                tipoUsuario = 3
                            }).ToList();

                        resultados.AddRange(alumnos);
                    }

                    if (resultados.Count == 0)
                    {
                        Console.WriteLine("No hay resultados");
                    }

                    foreach (var resultado in resultados)
                    {
                        Console.WriteLine("Resultado: " + resultado.nombre + " - Tipo: " + resultado.tipoUsuario);
                    }

                    return resultados.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar usuarios: {ex.Message}");
                return null;
            }
        }


        public UsuarioDTO EliminarUsuario(string correo, int tipoUsuario)
        {
            try
            {
                if (tipoUsuario == 1)
                {
                    var admin = usuarioDAO.ObtenerAdministradorPorCorreo(correo);
                    if (admin == null)
                    {
                        Console.WriteLine("Administrador no encontrado.");
                        return null;
                    }
                    UsuarioDTO dto = new UsuarioDTO
                    {
                        nombre = admin.nombre,
                        apellidos = admin.apellidos,
                        correo = admin.email,
                        telefono = admin.telefono,
                        direccion = admin.direccion,
                        tipoUsuario = 1
                    };
                    usuarioDAO.EliminarAdministrador(correo);
                    return dto;
                }
                else if (tipoUsuario == 2)
                {
                    var profesor = usuarioDAO.ObtenerProfesorPorCorreo(correo);
                    if (profesor == null)
                    {
                        Console.WriteLine("Profesor no encontrado.");
                        return null;
                    }
                    UsuarioDTO dto = new UsuarioDTO
                    {
                        nombre = profesor.nombre,
                        apellidos = profesor.apellidos,
                        correo = profesor.email,
                        telefono = profesor.telefono,
                        direccion = profesor.direccion,
                        tipoUsuario = 2
                    };
                    usuarioDAO.EliminarProfesor(correo);
                    return dto;
                }
                else if (tipoUsuario == 3)
                {
                    var alumno = usuarioDAO.ObtenerAlumnoPorCorreo(correo);
                    if (alumno == null)
                    {
                        Console.WriteLine("Alumno no encontrado.");
                        return null;
                    }
                    UsuarioDTO dto = new UsuarioDTO
                    {
                        nombre = alumno.nombre,
                        apellidos = alumno.apellidos,
                        correo = alumno.email,
                        telefono = alumno.telefono,
                        direccion = alumno.direccion,
                        tipoUsuario = 3
                    };
                    usuarioDAO.EliminarAlumno(correo);
                    return dto;
                }
                else
                {
                    Console.WriteLine("Tipo de usuario inválido.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
                return null;
            }
        }



        //Metodos IGestionUsuarios

        public CursoDTO ObtenerCursoPorID(int idCurso)
        {
            try
            {
                Curso curso = cursoDAO.ObtenerCursoPorID(idCurso);
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
                Curso[] cursos = cursoDAO.ObtenerCursos();
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
                Curso[] cursos = cursoDAO.ObtenerCursosPorNombre(nombre);
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
                Idioma idioma = idiomaDAO.ObtenerIdiomaPorID(idIdioma);
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
                Idioma[] idiomas = idiomaDAO.ObtenerIdiomas();
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
                Profesor[] profesores = profesorDAO.ObtenerProfesoresPorNombre(nombre);
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
                Profesor profesor = profesorDAO.ObtenerProfesorPorId(idProfesor);
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

                cursoDAO.CrearCurso(curso);
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
                Curso curso = cursoDAO.ObtenerCursoPorID(cursoDTO.idCurso);
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

                    cursoDAO.ModificarCurso(curso);
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
                Curso[] cursos = cursoDAO.ObtenerCursosPorNombreIdioma(nombreIdioma);
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
                Curso[] cursos = cursoDAO.ObtenerCursosPorNivel(nivel);
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
                Curso[] cursos = cursoDAO.ObtenerCursosPorNombreProfesor(nombreProfesor);
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

        public bool EliminarCurso(int idCurso)
        {
            try
            {
                cursoDAO.EliminarCurso(idCurso);
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

        public bool TieneAlumnosInscritos(int idCurso)
        {
            try
            {
                return cursoDAO.TieneAlumnosInscritos(idCurso);
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


        //Métodos de IGestionAlumnos

        public bool RegistrarInscripcion(InscripcionDTO inscripcionDTO)
        {
            try
            {
                Inscripcion inscripcion = new Inscripcion
                {
                    idCurso = inscripcionDTO.idCurso,
                    idAlumno = inscripcionDTO.idAlumno,
                    fechaInscripcion = inscripcionDTO.fechaInscripcion
                };
                inscripcionDAO.RegistrarInscripcion(inscripcion);
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

        public AlumnoDTO[] BuscarAlumnos(string nombre)
        {
            try
            {
                Alumno[] alumnos = usuarioDAO.ObtenerAlumnosPorNombre(nombre);
                if (alumnos != null)
                {
                    var alumnosDTO = alumnos.Select(a => new AlumnoDTO
                    {
                        idAlumno = a.idAlumno,
                        nombre = a.nombre,
                        apellidos = a.apellidos,
                        direccion = a.direccion,
                        email = a.email,
                        telefono = a.telefono
                    }).ToArray();
                    return alumnosDTO;
                }
                else
                {
                    Console.WriteLine("No se encontraron alumnos");
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

        public int ObtenerNumInscripciones(int idCurso)
        {
            try
            {
                return inscripcionDAO.ObtenerNumInscripciones(idCurso);
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                Console.WriteLine($"Error de Entity Framework: {ex.Message}");
                return -1;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return -1;
            }
        }

        public bool InscripcionExistente(int idCurso, int idAlumno)
        {
            try
            {
                if (inscripcionDAO.InscripcionExistente(idCurso, idAlumno))
                {
                    return true;
                }
                else
                {
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

        public bool AsignarHorario(HorarioDTO horario)
        {
            try
            {
                Horario h = new Horario
                {
                    idCurso = horario.idCurso,
                    dia = horario.diaSemana,
                    horaInicio = horario.horaInicio,
                    horaSalida = horario.horaTermino,
                };

                return horarioDAO.RegistrarHorario(h);
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

        public HorarioDTO[] ObtenerHorariosPorCurso(int idCurso)
        {
            try
            {
                Horario[] horarios = horarioDAO.RecuperarHorarios(idCurso);
                var horariosDTO = horarios.Select(h => new HorarioDTO
                {
                    idCurso = h.idCurso,
                    diaSemana = h.dia,
                    horaInicio = (TimeSpan)h.horaInicio,
                    horaTermino = (TimeSpan)h.horaSalida
                }).ToArray();
                return horariosDTO;
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
