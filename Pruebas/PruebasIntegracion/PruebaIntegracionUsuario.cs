using AccesoDatos.DAO;
using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Pruebas.PruebasIntegracion
{
    [TestFixture]
    public class PruebaIntegracionUsuario
    {
        private UsuarioDAO usuarioDAO;

        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config"));
            usuarioDAO = new UsuarioDAO();
        }

        [Test]
        public void CrearAdministrador_DeberiaAgregarAdministradorCorrectamente()
        {
            var admin = new Administrador
            {
                nombreUsuario = "AdminPrueba",
                contrasenia = "123456",
                nombre = "Administrador De Prueba",
                apellidos = "Prueba",
                email = "adminprueba@gmail.com",
                telefono = "2254897845",
                direccion = "Direccion, Prueba.",
                salt = Utilidades.GenerarSalt() 
            };

            Assert.DoesNotThrow(() => usuarioDAO.CrearAdministrador(admin));

            var resultado = usuarioDAO.ObtenerAdministradorPorCorreo("adminprueba@gmail.com");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Administrador De Prueba", resultado.nombre);
        }

        [Test]
        public void CrearProfesor_DeberiaAgregarProfesorCorrectamente()
        {
            var profesor = new Profesor
            {
                nombreUsuario = "profesorPrueba",
                contrasenia = "password123",
                nombre = "Profesor",
                apellidos = "Prueba",
                email = "profesorprueba@test.com",
                telefono = "2281697894",
                direccion = "Direccion, Prueba.",
                salt = Utilidades.GenerarSalt()
            };

            Assert.DoesNotThrow(() => usuarioDAO.CrearProfesor(profesor));

            var resultado = usuarioDAO.ObtenerProfesorPorCorreo("profesorprueba@test.com");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Profesor", resultado.nombre);
        }

        [Test]
        public void CrearAlumno_DeberiaAgregarAlumnoCorrectamente()
        {
            var alumno = new Alumno
            {
                nombre = "alumnoPrueba",
                apellidos = "Prueba",
                email = "alumnoprueba@test.com",
                telefono = "2281754896",
                direccion = "Direccion, Prueba."
            };

            Assert.DoesNotThrow(() => usuarioDAO.CrearAlumno(alumno));
        }

        [Test]
        public void ObtenerTipoUsuario_DeberiaRetornar1_ParaAdministrador()
        {
            var tipo = usuarioDAO.ObtenerTipoUsuario("adminprueba@gmail.com");
            Assert.AreEqual(1, tipo);
        }

        [Test]
        public void ObtenerTipoUsuario_DeberiaRetornar2_ParaProfesor()
        {
            var tipo = usuarioDAO.ObtenerTipoUsuario("juan.perez@example.com");
            Assert.AreEqual(2, tipo);
        }

        [Test]
        public void ObtenerTipoUsuario_DeberiaRetornar0_ParaUsuarioInexistente()
        {
            var tipo = usuarioDAO.ObtenerTipoUsuario("usuarioinexistente@test.com");
            Assert.AreEqual(0, tipo);
        }

        [Test]
        public void CrearRelacion_DeberiaCrearRelacionAlumnoIdiomaCorrectamente()
        {
            var alumno = new Alumno
            {
                nombre = "Angel Osvaldo",
                apellidos = "Lagunes Hernandez",
                email = "jp.lh@gmail.com",
                telefono = "2282569878",
                direccion = "Xalapa."
            };

            usuarioDAO.CrearAlumno(alumno);

            var relacion = new Idioma_Alumno
            {
                idAlumno = alumno.idAlumno,
                idIdioma = 1,
                nivel = 1
            };

            Assert.DoesNotThrow(() => usuarioDAO.CrearRelacionIdiomaAlumno(relacion));
        }

        [Test]
        public void ObtenerAdministradorPorCorreo_DeberiaRetornarAdministradorCorrecto()
        {
            var resultado = usuarioDAO.ObtenerAdministradorPorCorreo("axel.lu04@gmail.com");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Axel", resultado.nombre);
        }

        [Test]
        public void ObtenerAdministradorPorCorreo_DeberiaRetornarNull_SiNoExiste()
        {
            var resultado = usuarioDAO.ObtenerAdministradorPorCorreo("noexiste@test.com");
            Assert.IsNull(resultado);
        }

        [Test]
        public void ObtenerProfesorPorCorreo_DeberiaRetornarProfesorCorrecto()
        {
            var resultado = usuarioDAO.ObtenerProfesorPorCorreo("juan.perez@example.com");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.nombre);
        }

        [Test]
        public void ObtenerProfesorPorCorreo_DeberiaRetornarNull_SiNoExiste()
        {
            var resultado = usuarioDAO.ObtenerProfesorPorCorreo("profesornuevo@test.com");
            Assert.IsNull(resultado);
        }

        [Test]
        public void CrearRelacion_DeberiaAgregarRelacionCorrectamente()
        {
            var profesor = new Profesor
            {
                nombreUsuario = "JarlyPablo",
                contrasenia = "password123",
                nombre = "Jarly Pablo",
                apellidos = "Lagunes Hernandez",
                email = "jp.lh@gmail.com",
                telefono = "2282569878"
            };

            usuarioDAO.CrearProfesor(profesor);

            var relacion = new Idioma_Profesor
            {
                idProfesor = profesor.idProfesor,
                idIdioma = 1
            };

            Assert.DoesNotThrow(() => usuarioDAO.CrearRelacion(relacion));
        }
    }
}

