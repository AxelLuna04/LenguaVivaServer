using AccesoDatos;
using AccesoDatos.DAO;
using System;
using System.Collections.Generic;
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

        public CursoDTO ObtenerCursoPorID(int idCurso)
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

        // Metodos de IGestionCursos

        public Curso[] ObtenerCursos(string nombre)
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

        public CursoDTO[] ObtenerCursosPorNombre(string nombre)
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

        public IdiomaDTO ObtenerIdiomaPorID(int idIdioma)
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
            } else
            {
                Console.WriteLine("No se encontró el idioma con id: " + idIdioma);
                return null;
            }
        }

        public IdiomaDTO[] ObtenerIdiomas()
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

        public ProfesorDTO[] ObtenerProfesoresPorNombre(string nombre)
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

        public ProfesorDTO ObtenerProfesorPorID(int idProfesor)
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ModificarCurso(CursoDTO cursoDTO)
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
    }
}
