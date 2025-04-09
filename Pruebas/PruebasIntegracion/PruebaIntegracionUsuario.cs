using AccesoDatos.DAO;
using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ServicioContrato;

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

        [Test]
        public void EliminarAdministrador_DeberiaEliminarAdministradorCorrectamente()
        {
            var admin = new Administrador
            {
                nombreUsuario = "AdminEliminar",
                contrasenia = "123456",
                nombre = "Administrador Eliminar",
                apellidos = "Prueba",
                email = "admineliminar@example.com",
                telefono = "5551234567",
                direccion = "Dirección Test",
                salt = Utilidades.GenerarSalt()
            };

            usuarioDAO.CrearAdministrador(admin);

            Assert.DoesNotThrow(() => usuarioDAO.EliminarAdministrador(admin.email));

            var resultado = usuarioDAO.ObtenerAdministradorPorCorreo("admineliminar@example.com");
            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarProfesor_DeberiaEliminarProfesorCorrectamente()
        {
            var profesor = new Profesor
            {
                nombreUsuario = "ProfesorEliminar",
                contrasenia = "pass123",
                nombre = "Profesor Eliminar",
                apellidos = "Prueba",
                email = "profesoreliminar@example.com",
                telefono = "5559876543",
                direccion = "Dirección Test",
                salt = Utilidades.GenerarSalt()
            };

            usuarioDAO.CrearProfesor(profesor);

            Assert.DoesNotThrow(() => usuarioDAO.EliminarProfesor(profesor.email));

            var resultado = usuarioDAO.ObtenerProfesorPorCorreo("profesoreliminar@example.com");
            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarAlumno_DeberiaEliminarAlumnoCorrectamente()
        {
            var alumno = new Alumno
            {
                nombre = "Alumno Eliminar",
                apellidos = "Prueba",
                email = "alumnoeliminar@example.com",
                telefono = "5556789012",
                direccion = "Dirección Test"
            };

            usuarioDAO.CrearAlumno(alumno);

            Assert.DoesNotThrow(() => usuarioDAO.EliminarAlumno(alumno.email));

            var resultado = usuarioDAO.ObtenerAlumnoPorCorreo("alumnoeliminar@example.com");
            Assert.IsNull(resultado);
        }

        [Test]
        public void EliminarUsuario_NoDeberiaFallar_SiUsuarioNoExiste()
        {
            Assert.DoesNotThrow(() => usuarioDAO.EliminarAdministrador("noexiste@example.com"));
            Assert.DoesNotThrow(() => usuarioDAO.EliminarProfesor("noexiste@example.com"));
            Assert.DoesNotThrow(() => usuarioDAO.EliminarAlumno("noexiste@example.com"));
        }

        [Test]
        public void ModificarAdministrador_DeberiaModificarAdministradorCorrectamente()
        {
            // Arrange
            var admin = new Administrador
            {
                nombreUsuario = "AdminExistente",
                contrasenia = "123456",
                nombre = "Admin Antiguo",
                apellidos = "Antiguo",
                email = "adminexistente@test.com",
                telefono = "2254897845",
                direccion = "Direccion antigua.",
                salt = Utilidades.GenerarSalt()
            };

            // Crear el administrador
            usuarioDAO.CrearAdministrador(admin);

            // Modificar la información
            var adminModificado = new AdministradorDTO
            {
                email = "adminexistente@test.com",
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevacontrasenia"
            };

            // Act

            // Assert

            var adminResultado = usuarioDAO.ObtenerAdministradorPorCorreo("adminexistente@test.com");
            Assert.IsNotNull(adminResultado);
            Assert.AreEqual("Nuevo Nombre", adminResultado.nombre);
            Assert.AreEqual("Nuevo Apellido", adminResultado.apellidos);
            Assert.AreEqual("Nueva Direccion", adminResultado.direccion);
            Assert.AreEqual("5559876543", adminResultado.telefono);
            Assert.AreNotEqual("123456", adminResultado.contrasenia); // Contraseña modificada
        }

        [Test]
        public void ModificarAdministrador_DeberiaRetornarFalseSiNoExisteAdministrador()
        {
            // Arrange
            var adminModificado = new AdministradorDTO
            {
                email = "noexiste@test.com", // Este administrador no existe
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevacontrasenia"
            };
        }

        [Test]
        public void ModificarAdministrador_DeberiaRetornarFalseSiOcurreError()
        {
            // Arrange
            var adminModificado = new AdministradorDTO
            {
                email = "adminexistente@test.com",
                nombre = "Nuevo Nombre",
                apellidos = "Nuevo Apellido",
                direccion = "Nueva Direccion",
                telefono = "5559876543",
                contrasenia = "nuevacontrasenia"
            };

            // Simulamos que ocurre un error en la base de datos al modificar el administrador
            // Se puede usar un mock para simular una excepción si lo estás haciendo con Mocking en vez de una base de datos real
            var exceptionMessage = "Error al modificar el administrador";
      

            // Assert
        }

        [Test]
        public void ObtenerAdministradorPorCorreoDeberiaRetornarAdministradorCorrecto()
        {
            var admin = new Administrador
            {
                nombreUsuario = "adminIntegration",
                contrasenia = "admin123",
                nombre = "Carlos",
                apellidos = "López",
                email = "adminintegration@test.com",
                telefono = "1234567890",
                direccion = "Calle Prueba 123",
                salt = Utilidades.GenerarSalt()
            };

            usuarioDAO.CrearAdministrador(admin);

            var resultado = usuarioDAO.ObtenerAdministradorPorCorreo("adminintegration@test.com");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Carlos", resultado.nombre);
            Assert.AreEqual("adminintegration@test.com", resultado.email);
        }

        [Test]
        public void ObtenerProfesorPorCorreoDeberiaRetornarProfesorCorrecto()
        {
            var profesor = new Profesor
            {
                nombreUsuario = "profeIntegration",
                contrasenia = "profe123",
                nombre = "Lucía",
                apellidos = "Ramírez",
                email = "profeintegration@test.com",
                telefono = "9988776655",
                direccion = "Avenida Docente 456",
                salt = Utilidades.GenerarSalt()
            };

            usuarioDAO.CrearProfesor(profesor);

            var resultado = usuarioDAO.ObtenerProfesorPorCorreo("profeintegration@test.com");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Lucía", resultado.nombre);
            Assert.AreEqual("profeintegration@test.com", resultado.email);
        }

        [Test]
        public void ObtenerAlumnoPorCorreo_DeberiaRetornarAlumnoCorrecto()
        {
            var alumno = new Alumno
            {
                nombre = "Marco",
                apellidos = "González",
                email = "alumnointegration@test.com",
                telefono = "1122334455",
                direccion = "Calle Estudiante 789"
            };

            usuarioDAO.CrearAlumno(alumno);

            var resultado = usuarioDAO.ObtenerAlumnoPorCorreo("alumnointegration@test.com");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Marco", resultado.nombre);
            Assert.AreEqual("alumnointegration@test.com", resultado.email);
        }

        [Test]
        public void ModificarProfesor_DeberiaActualizarProfesorCorrectamente()
        {
            // Arrange
            var profesorOriginal = new Profesor
            {
                idProfesor = 1,
                nombreUsuario = "profesorOriginal",
                contrasenia = "oldpassword123",
                nombre = "Profesor Original",
                apellidos = "Prueba",
                email = "profesororiginal@test.com",
                telefono = "2281697894",
                direccion = "Direccion, Prueba.",
                salt = Utilidades.GenerarSalt()
            };

            usuarioDAO.CrearProfesor(profesorOriginal);

            var profesorDTO = new ProfesorDTO
            {
                idProfesor = profesorOriginal.idProfesor,
                nombre = "Profesor Modificado",
                apellidos = "Modificado",
                direccion = "Direccion modificada",
                telefono = "2281691234",
                contrasenia = "newpassword123"
            };

            // Act
            //Assert.DoesNotThrow(() => usuarioDAO.ModificarProfesor(profesorDTO));

            // Assert
            var resultado = usuarioDAO.ObtenerProfesorPorId(profesorOriginal.idProfesor);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Profesor Modificado", resultado.nombre);
            Assert.AreEqual("Direccion modificada", resultado.direccion);
            Assert.AreEqual("2281691234", resultado.telefono);
        }

        [Test]
        public void ModificarAlumno_DeberiaActualizarAlumnoCorrectamente()
        {
            // Arrange
            var alumnoOriginal = new Alumno
            {
                idAlumno = 1,
                nombre = "Alumno Original",
                apellidos = "Prueba",
                email = "alumnooriginal@test.com",
                telefono = "2281754896",
                direccion = "Direccion, Prueba."
            };

            usuarioDAO.CrearAlumno(alumnoOriginal);

            var alumnoDTO = new AlumnoDTO
            {
                idAlumno = alumnoOriginal.idAlumno,
                nombre = "Alumno Modificado",
                apellidos = "Modificado",
                direccion = "Direccion modificada",
                telefono = "2281691234"
            };

            var idiomasNivel = new IdiomaNivelDTO[]
            {
                new IdiomaNivelDTO { idIdioma = 1, nivel = 2 }
            };

            // Act
            //Assert.DoesNotThrow(() => usuarioDAO.ModificarAlumno(alumnoDTO, idiomasNivel));

            // Assert
            var resultado = usuarioDAO.ObtenerAlumnoPorId(alumnoOriginal.idAlumno);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Alumno Modificado", resultado.nombre);
            Assert.AreEqual("Direccion modificada", resultado.direccion);
            Assert.AreEqual("2281691234", resultado.telefono);
        }

        [Test]
        public void ModificarProfesor_ErrorBaseDatos_DevuelveException()
        {
            // Arrange
            var profesorDTO = new ProfesorDTO
            {
                idProfesor = 999, // Profesor inexistente
                nombre = "Profesor Inexistente"
            };

            // Act & Assert
            //Assert.Throws<Exception>(() => usuarioDAO.ModificarProfesorCompleto(profesorDTO));
        }

        [Test]
        public void ModificarAlumno_ErrorBaseDatos_DevuelveException()
        {
            // Arrange
            var alumnoDTO = new AlumnoDTO
            {
                idAlumno = 999, // Alumno inexistente
                nombre = "Alumno Inexistente"
            };

            var idiomasNivel = new IdiomaNivelDTO[]
            {
                new IdiomaNivelDTO { idIdioma = 1, nivel = 2 }
            };

            // Act & Assert
            //Assert.Throws<Exception>(() => usuarioDAO.ModificarAlumnoCompleto(alumnoDTO, idiomasNivel));
        }


    }
}

