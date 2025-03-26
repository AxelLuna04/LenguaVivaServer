using NUnit.Framework;
using ServicioContrato;
using AccesoDatos.DAO;
using AccesoDatos;
using Moq;
using System.Data.SqlClient;
using System.Data.Entity;
using AccesoDatos.Interfaces;
using System.Data.Entity.Core;

#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
namespace PruebasUnitarias
{
    [TestFixture]
    public class ImplementacionServicioTestsUsuarios
    {
        private Mock<IUsuarioDAO> mockDAO;
        private Mock<IIdiomaDAO> mockIdiomaDAO;
        private ImplementacionServicio service;

        [SetUp]
        public void Setup()
        {
            mockDAO = new Mock<IUsuarioDAO>();
            mockIdiomaDAO = new Mock<IIdiomaDAO>();
            service = new ImplementacionServicio(mockIdiomaDAO.Object, null, mockDAO.Object, null);
        }

        [Test]
        public void IniciarSesion_AdministradorValido_DevuelveSesionValida()
        {
            // Arrange
            var salt = Utilidades.GenerarSalt();
            var admin = new Administrador
            {
                email = "admin@test.com",
                contrasenia = Utilidades.HashearContrasena("pass123", salt),
                nombre = "Admin",
                apellidos = "Test",
                salt = salt
            };

            mockDAO.Setup(dao => dao.ObtenerTipoUsuario("admin@test.com")).Returns(1);
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@test.com")).Returns(admin);

            // Act
            var resultado = service.IniciarSesion("admin@test.com", "pass123");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.esValido, Is.True);
            Assert.That(resultado.tipoUsuario, Is.EqualTo(1));
            Assert.That(resultado.correo, Is.EqualTo("admin@test.com"));
        }

        [Test]
        public void IniciarSesion_ProfesorValido_DevuelveSesionValida()
        {
            // Arrange
            var salt = Utilidades.GenerarSalt();
            var profesor = new Profesor
            {
                email = "profesor@test.com",
                contrasenia = Utilidades.HashearContrasena("pass123", salt),
                nombre = "Profesor",
                apellidos = "Test",
                salt = salt
            };

            mockDAO.Setup(dao => dao.ObtenerTipoUsuario("profesor@test.com")).Returns(2);
            mockDAO.Setup(dao => dao.ObtenerProfesorPorCorreo("profesor@test.com")).Returns(profesor);

            // Act
            var resultado = service.IniciarSesion("profesor@test.com", "pass123");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.esValido, Is.True);
            Assert.That(resultado.tipoUsuario, Is.EqualTo(2));
            Assert.That(resultado.correo, Is.EqualTo("profesor@test.com"));
        }


        [Test]
        public void IniciarSesion_CredencialesInvalidas_DevuelveSesionInvalida()
        {
            // Arrange
            var admin = new Administrador
            {
                email = "admin@test.com",
                contrasenia = "pass123"
            };

            mockDAO.Setup(dao => dao.ObtenerTipoUsuario("admin@test.com")).Returns(1);
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@test.com")).Returns(admin);

            // Act
            var resultado = service.IniciarSesion("admin@test.com", "passwordIncorrecta");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.esValido, Is.False);
        }

        [Test]
        public void RegistrarAdministrador_CreaAdministradorCorrectamente()
        {
            // Arrange
            Administrador adminGuardado = null;

            mockDAO.Setup(dao => dao.CrearAdministrador(It.IsAny<Administrador>()))
                   .Callback<Administrador>(admin => adminGuardado = admin);

            var adminDTO = new AdministradorDTO
            {
                nombreUsuario = "admin1",
                contrasenia = "pass123",
                nombre = "Admin",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "admin@test.com",
                telefono = "5551234567"
            };

            // Act
            var resultado = service.RegistrarAdministrador(adminDTO);

            // Assert
            Assert.That(resultado, Is.True);
            Assert.That(adminGuardado, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(adminGuardado.nombreUsuario, Is.EqualTo("admin1"));
                //Assert.That(adminGuardado.contrasenia, Is.EqualTo("pass123"));
                Assert.That(adminGuardado.nombre, Is.EqualTo("Admin"));
                Assert.That(adminGuardado.apellidos, Is.EqualTo("Test"));
                Assert.That(adminGuardado.direccion, Is.EqualTo("Calle 123"));
                Assert.That(adminGuardado.email, Is.EqualTo("admin@test.com"));
                Assert.That(adminGuardado.telefono, Is.EqualTo("5551234567"));
            });
        }

        [Test]
        public void RegistrarAdministrador_ErrorBaseDatos_DevuelveFalse()
        {
            // Arrange
            mockDAO.Setup(dao => dao.CrearAdministrador(It.IsAny<Administrador>()))
                   .Throws(new EntityException("Error de base de datos"));

            var adminDTO = new AdministradorDTO
            {
                nombreUsuario = "admin1",
                contrasenia = "pass123",
                nombre = "Admin",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "admin@test.com",
                telefono = "5551234567"
            };

            // Act
            var resultado = service.RegistrarAdministrador(adminDTO);

            // Assert
            Assert.That(resultado, Is.False);
        }

        [Test]
        public void RegistrarProfesor_CreaProfesorCorrectamente()
        {
            // Arrange
            Profesor profesorGuardado = null;
            List<Idioma_Profesor> relacionesGuardadas = new List<Idioma_Profesor>();

            mockDAO.Setup(dao => dao.CrearProfesor(It.IsAny<Profesor>()))
                   .Callback<Profesor>(prof => profesorGuardado = prof);

            mockDAO.Setup(dao => dao.CrearRelacion(It.IsAny<Idioma_Profesor>()))
                   .Callback<Idioma_Profesor>(rel => relacionesGuardadas.Add(rel));

            var profesorDTO = new ProfesorDTO
            {
                nombreUsuario = "prof1",
                contrasenia = "pass123",
                nombre = "Profesor",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "prof@test.com",
                telefono = "5551234567"
            };

            int[] idiomas = { 1, 2 };

            // Act
            var resultado = service.RegistrarProfesor(profesorDTO, idiomas);

            // Assert
            Assert.That(resultado, Is.True);
            Assert.That(profesorGuardado, Is.Not.Null);
            Assert.That(relacionesGuardadas.Count, Is.EqualTo(2));

            Assert.Multiple(() =>
            {
                Assert.That(profesorGuardado.nombreUsuario, Is.EqualTo("prof1"));
                //Assert.That(profesorGuardado.contrasenia, Is.EqualTo("pass123"));
                Assert.That(profesorGuardado.nombre, Is.EqualTo("Profesor"));
                Assert.That(profesorGuardado.apellidos, Is.EqualTo("Test"));
            });
        }

        [Test]
        public void RegistrarProfesor_ErrorBaseDatos_DevuelveFalse()
        {
            // Arrange
            mockDAO.Setup(dao => dao.CrearProfesor(It.IsAny<Profesor>()))
                   .Throws(new EntityException("Error de base de datos"));

            var profesorDTO = new ProfesorDTO
            {
                nombreUsuario = "prof1",
                contrasenia = "pass123",
                nombre = "Profesor",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "prof@test.com",
                telefono = "5551234567"
            };

            int[] idiomas = { 1, 2 };

            // Act
            var resultado = service.RegistrarProfesor(profesorDTO, idiomas);

            // Assert
            Assert.That(resultado, Is.False);
        }

        [Test]
        public void CargarIdiomas_DevuelveListaIdiomas()
        {
            // Arrange
            var idiomas = new[]
            {
                new Idioma { idIdioma = 1, nombreIdioma = "Inglés" },
                new Idioma { idIdioma = 2, nombreIdioma = "Español" }
            };

            mockIdiomaDAO.Setup(dao => dao.CargarIdiomas()).Returns(idiomas);

            // Act
            var resultado = service.CargarIdiomas();

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Length, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(resultado[0].idIdioma, Is.EqualTo(1));
                Assert.That(resultado[0].nombreIdioma, Is.EqualTo("Inglés"));
                Assert.That(resultado[1].idIdioma, Is.EqualTo(2));
                Assert.That(resultado[1].nombreIdioma, Is.EqualTo("Español"));
            });
        }

        [Test]
        public void CargarIdiomas_ErrorBaseDatos_DevuelveNull()
        {
            // Arrange
            mockIdiomaDAO.Setup(dao => dao.CargarIdiomas())
                   .Throws(new EntityException("Error de base de datos"));

            // Act
            var resultado = service.CargarIdiomas();

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void RegistrarAlumno_CreaAlumnoCorrectamente()
        {
            // Arrange
            Alumno alumnoGuardado = null;
            List<Idioma_Alumno> relacionesGuardadas = new List<Idioma_Alumno>();

            mockDAO.Setup(dao => dao.CrearAlumno(It.IsAny<Alumno>()))
                   .Callback<Alumno>(alumno => alumnoGuardado = alumno);

            mockDAO.Setup(dao => dao.CrearRelacionIdiomaAlumno(It.IsAny<Idioma_Alumno>()))
                   .Callback<Idioma_Alumno>(rel => relacionesGuardadas.Add(rel));

            var alumnoDTO = new AlumnoDTO
            {
                nombre = "Alumno",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "alumno@test.com",
                telefono = "5551234567"
            };

            var idiomasNivel = new[]
            {
                new IdiomaNivelDTO { idIdioma = 1, nivel = 2 },
                new IdiomaNivelDTO { idIdioma = 2, nivel = 1 }
            };

            // Act
            var resultado = service.RegistrarAlumno(alumnoDTO, idiomasNivel);

            // Assert
            Assert.That(resultado, Is.True);
            Assert.That(alumnoGuardado, Is.Not.Null);
            Assert.That(relacionesGuardadas.Count, Is.EqualTo(2));

            Assert.Multiple(() =>
            {
                Assert.That(alumnoGuardado.nombre, Is.EqualTo("Alumno"));
                Assert.That(alumnoGuardado.apellidos, Is.EqualTo("Test"));
                Assert.That(relacionesGuardadas[0].idIdioma, Is.EqualTo(1));
                Assert.That(relacionesGuardadas[0].nivel, Is.EqualTo(2));
                Assert.That(relacionesGuardadas[1].idIdioma, Is.EqualTo(2));
                Assert.That(relacionesGuardadas[1].nivel, Is.EqualTo(1));
            });
        }

        [Test]
        public void RegistrarAlumno_ErrorBaseDatos_DevuelveFalse()
        {
            // Arrange
            mockDAO.Setup(dao => dao.CrearAlumno(It.IsAny<Alumno>()))
                   .Throws(new EntityException("Error de base de datos"));

            var alumnoDTO = new AlumnoDTO
            {
                nombre = "Alumno",
                apellidos = "Test",
                direccion = "Calle 123",
                email = "alumno@test.com",
                telefono = "5551234567"
            };

            var idiomasNivel = new[]
            {
                new IdiomaNivelDTO { idIdioma = 1, nivel = 2 }
            };

            // Act
            var resultado = service.RegistrarAlumno(alumnoDTO, idiomasNivel);

            // Assert
            Assert.That(resultado, Is.False);
        }
    }
}