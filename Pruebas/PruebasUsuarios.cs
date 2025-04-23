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
            service = new ImplementacionServicio(mockIdiomaDAO.Object, null, mockDAO.Object, null, null, null, null);
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

        [Test]
        public void EliminarUsuario_AdministradorExiste_EliminaCorrectamente()
        {
            string correo = "admin@test.com";
            var admin = new Administrador { nombre = "Juan", apellidos = "Pérez", email = correo, telefono = "123456789", direccion = "Calle 123" };
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo(correo)).Returns(admin);
            mockDAO.Setup(dao => dao.EliminarAdministrador(correo));

            var resultado = service.EliminarUsuario(correo, 1);

            Assert.NotNull(resultado);
            Assert.That(resultado.nombre, Is.EqualTo("Juan"));
            mockDAO.Verify(dao => dao.EliminarAdministrador(correo), Times.Once);
        }

        [Test]
        public void EliminarUsuario_ProfesorExiste_EliminaCorrectamente()
        {
            string correo = "profesor@test.com";
            var profesor = new Profesor { nombre = "Ana", apellidos = "Gómez", email = correo, telefono = "987654321", direccion = "Avenida 456" };
            mockDAO.Setup(dao => dao.ObtenerProfesorPorCorreo(correo)).Returns(profesor);
            mockDAO.Setup(dao => dao.EliminarProfesor(correo));

            var resultado = service.EliminarUsuario(correo, 2);

            Assert.NotNull(resultado);
            Assert.That(resultado.nombre, Is.EqualTo("Ana"));
            mockDAO.Verify(dao => dao.EliminarProfesor(correo), Times.Once);
        }

        [Test]
        public void EliminarUsuario_AlumnoExiste_EliminaCorrectamente()
        {
            string correo = "alumno@test.com";
            var alumno = new Alumno { nombre = "Pedro", apellidos = "Martínez", email = correo, telefono = "111222333", direccion = "Plaza 789" };
            mockDAO.Setup(dao => dao.ObtenerAlumnoPorCorreo(correo)).Returns(alumno);
            mockDAO.Setup(dao => dao.EliminarAlumno(correo));

            var resultado = service.EliminarUsuario(correo, 3);

            Assert.NotNull(resultado);
            Assert.That(resultado.nombre, Is.EqualTo("Pedro"));
            mockDAO.Verify(dao => dao.EliminarAlumno(correo), Times.Once);
        }

        [Test]
        public void EliminarUsuario_AdministradorNoExiste_RetornaNull()
        {
            string correo = "admin@test.com";
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo(correo)).Returns((Administrador)null);

            var resultado = service.EliminarUsuario(correo, 1);

            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarUsuario_ProfesorNoExiste_RetornaNull()
        {
            string correo = "profesor@test.com";
            mockDAO.Setup(dao => dao.ObtenerProfesorPorCorreo(correo)).Returns((Profesor)null);

            var resultado = service.EliminarUsuario(correo, 2);

            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarUsuario_AlumnoNoExiste_RetornaNull()
        {
            string correo = "alumno@test.com";
            mockDAO.Setup(dao => dao.ObtenerAlumnoPorCorreo(correo)).Returns((Alumno)null);

            var resultado = service.EliminarUsuario(correo, 3);

            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarUsuario_TipoUsuarioInvalido_RetornaNull()
        {
            string correo = "usuario@test.com";

            var resultado = service.EliminarUsuario(correo, 99);

            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarUsuario_ErrorBaseDatos_LanzaExcepcion()
        {
            string correo = "admin@test.com";
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo(correo)).Throws(new EntityException("Error de base de datos"));

            var resultado = service.EliminarUsuario(correo, 1);

            Assert.IsNull(resultado);
        }

        [Test]
        public void ModificarAdministrador_AdministradorExistente_DevuelveTrue()
        {
            // Arrange
            var administradorDTO = new AdministradorDTO
            {
                email = "admin@test.com",
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevaContraseña"
            };

            var administradorExistente = new Administrador
            {
                email = "admin@test.com",
                nombre = "Admin",
                apellidos = "Test",
                direccion = "Calle 123",
                telefono = "5551234567",
                contrasenia = "pass123",
                salt = "salt"
            };

            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@test.com")).Returns(administradorExistente);
            mockDAO.Setup(dao => dao.ModificarAdministrador(It.IsAny<Administrador>())).Verifiable();

            // Act
            var resultado = service.ModificarAdministrador(administradorDTO);

            // Assert
            Assert.That(resultado, Is.True);
            Assert.That(administradorExistente.nombre, Is.EqualTo("Nuevo Nombre"));
            Assert.That(administradorExistente.apellidos, Is.EqualTo("Nuevo Apellido"));
            Assert.That(administradorExistente.direccion, Is.EqualTo("Nueva Direccion"));
            Assert.That(administradorExistente.telefono, Is.EqualTo("5559876543"));
            Assert.That(administradorExistente.contrasenia, Is.Not.EqualTo("pass123"));
            mockDAO.Verify(dao => dao.ModificarAdministrador(It.IsAny<Administrador>()), Times.Once);
        }

        [Test]
        public void ModificarAdministrador_AdministradorNoExistente_DevuelveFalse()
        {
            // Arrange
            var administradorDTO = new AdministradorDTO
            {
                email = "noexiste@test.com",
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevaContraseña"
            };

            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("noexiste@test.com")).Returns((Administrador)null);

            // Act
            var resultado = service.ModificarAdministrador(administradorDTO);

            // Assert
            Assert.That(resultado, Is.False);
        }

        [Test]
        public void ModificarAdministrador_ErrorAlModificar_DevuelveFalse()
        {
            // Arrange
            var administradorDTO = new AdministradorDTO
            {
                email = "admin@test.com",
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevaContraseña"
            };

            var administradorExistente = new Administrador
            {
                email = "admin@test.com",
                nombre = "Admin",
                apellidos = "Test",
                direccion = "Calle 123",
                telefono = "5551234567",
                contrasenia = "pass123",
                salt = "salt"
            };

            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@test.com")).Returns(administradorExistente);
            mockDAO.Setup(dao => dao.ModificarAdministrador(It.IsAny<Administrador>())).Throws(new Exception("Error al modificar"));

            // Act
            var resultado = service.ModificarAdministrador(administradorDTO);

            // Assert
            Assert.That(resultado, Is.False);
        }

        [Test]
        public void ObtenerAdministradorPorCorreo_ExisteCorreo_DevuelveDTO()
        {
            // Arrange
            var admin = new Administrador
            {
                idAdministrador = 1,
                nombreUsuario = "admin123",
                nombre = "Carlos",
                apellidos = "Pérez",
                direccion = "Calle Falsa 123",
                email = "admin@test.com",
                telefono = "1234567890"
            };

            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@test.com")).Returns(admin);

            // Act
            var resultado = service.ObtenerAdministradorPorCorreo("admin@test.com");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.idAdministrador, Is.EqualTo(1));
            Assert.That(resultado.nombre, Is.EqualTo("Carlos"));
            Assert.That(resultado.email, Is.EqualTo("admin@test.com"));
        }

        [Test]
        public void ObtenerAdministradorPorCorreo_NoExisteCorreo_DevuelveNull()
        {
            // Arrange
            mockDAO.Setup(dao => dao.ObtenerAdministradorPorCorreo("admin@desconocido.com")).Returns((Administrador)null);

            // Act
            var resultado = service.ObtenerAdministradorPorCorreo("admin@desconocido.com");

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerProfesorPorCorreo_ExisteCorreo_DevuelveDTO()
        {
            // Arrange
            var profesor = new Profesor
            {
                idProfesor = 2,
                nombreUsuario = "profe321",
                nombre = "Laura",
                apellidos = "García",
                direccion = "Av. Universidad 456",
                email = "profesor@test.com",
                telefono = "987654321"
            };

            mockDAO.Setup(dao => dao.ObtenerProfesorPorCorreo("profesor@test.com")).Returns(profesor);

            // Act
            var resultado = service.ObtenerProfesorPorCorreo("profesor@test.com");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.idProfesor, Is.EqualTo(2));
            Assert.That(resultado.nombre, Is.EqualTo("Laura"));
            Assert.That(resultado.email, Is.EqualTo("profesor@test.com"));
        }

        [Test]
        public void ObtenerProfesorPorCorreo_NoExisteCorreo_DevuelveNull()
        {
            // Arrange
            mockDAO.Setup(dao => dao.ObtenerProfesorPorCorreo("profesor@desconocido.com")).Returns((Profesor)null);

            // Act
            var resultado = service.ObtenerProfesorPorCorreo("profesor@desconocido.com");

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerAlumnoPorCorreo_ExisteCorreo_DevuelveDTO()
        {
            // Arrange
            var alumno = new Alumno
            {
                idAlumno = 3,
                nombre = "Ana",
                apellidos = "Martínez",
                direccion = "Colonia Centro",
                email = "alumno@test.com",
                telefono = "555666777"
            };

            mockDAO.Setup(dao => dao.ObtenerAlumnoPorCorreo("alumno@test.com")).Returns(alumno);

            // Act
            var resultado = service.ObtenerAlumnoPorCorreo("alumno@test.com");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.idAlumno, Is.EqualTo(3));
            Assert.That(resultado.nombre, Is.EqualTo("Ana"));
            Assert.That(resultado.email, Is.EqualTo("alumno@test.com"));
        }

        [Test]
        public void ObtenerAlumnoPorCorreo_NoExisteCorreo_DevuelveNull()
        {
            // Arrange
            mockDAO.Setup(dao => dao.ObtenerAlumnoPorCorreo("alumno@desconocido.com")).Returns((Alumno)null);

            // Act
            var resultado = service.ObtenerAlumnoPorCorreo("alumno@desconocido.com");

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ModificarProfesor_ProfesorExistente_ActualizaProfesor()
        {
            // Arrange
            var profesorDTO = new ProfesorDTO
            {
                idProfesor = 1,
                nombre = "Juan",
                apellidos = "Perez",
                direccion = "Calle Ficticia 123",
                telefono = "5551234567",
                contrasenia = "newPass123"
            };

            var profesorEntity = new Profesor
            {
                idProfesor = 1,
                nombre = "Juan",
                apellidos = "Perez",
                direccion = "Calle Ficticia 123",
                telefono = "5551234567",
                contrasenia = Utilidades.HashearContrasena("oldPass123", Utilidades.GenerarSalt()),
                salt = "oldSalt"
            };

            mockDAO.Setup(dao => dao.ObtenerProfesorPorId(profesorDTO.idProfesor)).Returns(profesorEntity);
            //mockDAO.Setup(dao => dao.ModificarProfesorCompleto(It.IsAny<Profesor>(), It.IsAny<int[]>())).Verifiable();

            // Act
            var resultado = service.ModificarProfesor(profesorDTO);

            // Assert
            Assert.That(resultado, Is.True);
            //mockDAO.Verify(dao => dao.ModificarProfesorCompleto(It.IsAny<Profesor>(), It.IsAny<int[]>()), Times.Once);
        }

        [Test]
        public void ModificarProfesor_ProfesorNoExistente_NoActualizaProfesor()
        {
            // Arrange
            var profesorDTO = new ProfesorDTO
            {
                idProfesor = 1,
                nombre = "Juan",
                apellidos = "Perez",
                direccion = "Calle Ficticia 123",
                telefono = "5551234567"
            };

            mockDAO.Setup(dao => dao.ObtenerProfesorPorId(profesorDTO.idProfesor)).Returns((Profesor)null);

            // Act
            var resultado = service.ModificarProfesor(profesorDTO);

            // Assert
            Assert.That(resultado, Is.False);
        }

        [Test]
        public void ModificarAlumno_AlumnoExistente_ActualizaAlumno()
        {
            // Arrange
            var alumnoDTO = new AlumnoDTO
            {
                idAlumno = 1,
                nombre = "Carlos",
                apellidos = "Sanchez",
                direccion = "Calle Ejemplo 456",
                telefono = "5559876543"
            };

            var idiomasNivel = new IdiomaNivelDTO[]
            {
            new IdiomaNivelDTO { idIdioma = 1, idiomaNombre = "Inglés", nivel = 3 },
            new IdiomaNivelDTO { idIdioma = 2, idiomaNombre = "Francés", nivel = 2 }
            };

            var alumnoEntity = new Alumno
            {
                idAlumno = 1,
                nombre = "Carlos",
                apellidos = "Sanchez",
                direccion = "Calle Ejemplo 456",
                telefono = "5559876543"
            };

            mockDAO.Setup(dao => dao.ObtenerAlumnoPorId(alumnoDTO.idAlumno)).Returns(alumnoEntity);
            mockDAO.Setup(dao => dao.ModificarAlumnoCompleto(It.IsAny<Alumno>(), It.IsAny<List<Idioma_Alumno>>())).Verifiable();

            // Act
            var resultado = service.ModificarAlumno(alumnoDTO, idiomasNivel);

            // Assert
            Assert.That(resultado, Is.True);
            mockDAO.Verify(dao => dao.ModificarAlumnoCompleto(It.IsAny<Alumno>(), It.IsAny<List<Idioma_Alumno>>()), Times.Once);
        }

        [Test]
        public void ModificarAlumno_AlumnoNoExistente_NoActualizaAlumno()
        {
            // Arrange
            var alumnoDTO = new AlumnoDTO
            {
                idAlumno = 1,
                nombre = "Carlos",
                apellidos = "Sanchez",
                direccion = "Calle Ejemplo 456",
                telefono = "5559876543"
            };

            var idiomasNivel = new IdiomaNivelDTO[]
            {
            new IdiomaNivelDTO { idIdioma = 1, idiomaNombre = "Inglés", nivel = 3 }
            };

            mockDAO.Setup(dao => dao.ObtenerAlumnoPorId(alumnoDTO.idAlumno)).Returns((Alumno)null);

            // Act
            var resultado = service.ModificarAlumno(alumnoDTO, idiomasNivel);

            // Assert
            Assert.That(resultado, Is.False);
        }

    }
}