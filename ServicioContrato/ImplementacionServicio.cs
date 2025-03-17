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
                    estado = "Propuesto",
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
    }
}
