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
    public class PruebasCursos
    {

        private Mock<ICursoDAO> mockDAO;
        private ImplementacionServicio service;


        [SetUp]
        public void Setup()
        {
            mockDAO = new Mock<ICursoDAO>();
            service = new ImplementacionServicio(null, mockDAO.Object, null, null, null, null, null);
        }

        [Test]
        public void RegistrarCurso_CreaCursoCorrectamente()
        {
            // Arrange
            Curso cursoGuardado = null;

            mockDAO.Setup(dao => dao.CrearCurso(It.IsAny<Curso>()))
                   .Callback<Curso>(curso => cursoGuardado = curso);

            var cursoPrueba = new CursoDTO
            {
                nombreCurso = "Curso de prueba",
                nivel = 1,
                descripcion = "Descripción de prueba",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(10),
                cupoMaximo = 10,
                idIdioma = 1,
                idProfesor = 1
            };

            // Act
            service.RegistrarCurso(cursoPrueba);

            // Assert
            Assert.That(cursoGuardado, Is.Not.Null, "El curso debería haber sido creado.");
            Assert.Multiple(() =>
            {
                Assert.That(cursoGuardado.nombreCurso, Is.EqualTo("Curso de prueba"));
                Assert.That(cursoGuardado.nivel, Is.EqualTo(1));
                Assert.That(cursoGuardado.descripcion, Is.EqualTo("Descripción de prueba"));
                Assert.That(cursoGuardado.cupoMaximo, Is.EqualTo(10));
                Assert.That(cursoGuardado.idIdioma, Is.EqualTo(1));
                Assert.That(cursoGuardado.idProfesor, Is.EqualTo(1));
            });

            mockDAO.Verify(dao => dao.CrearCurso(It.IsAny<Curso>()), Times.Once, "El método CrearCurso debería haber sido invocado una vez.");
        }

        [Test]
        public void EditarCurso_ModificaCursoCorrectamente()
        {
            // Arrange
            var cursoExistente = new Curso
            {
                idCurso = 1,
                nombreCurso = "Curso de prueba",
                nivel = 1,
                descripcion = "Descripción de prueba",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(30),
                cupoMaximo = 10,
                estado = "Activo",
                idIdioma = 1,
                idProfesor = 1
            };

            mockDAO.Setup(dao => dao.ObtenerCursoPorID(1))
                   .Returns(cursoExistente);

            Curso cursoModificado = null;

            mockDAO.Setup(dao => dao.ModificarCurso(It.IsAny<Curso>()))
                   .Callback<Curso>(curso => cursoModificado = curso);

            var cursoDTO = new CursoDTO
            {
                idCurso = 1,
                nombreCurso = "Curso actualizado",
                descripcion = "Descripción modificada",
                nivel = 2,
                cupoMaximo = 25,
                estado = "Inactivo",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(45),
                idIdioma = 2,
                idProfesor = 3
            };

            // Act
            var resultado = service.ModificarCurso(cursoDTO);

            // Assert
            Assert.That(resultado, Is.True, "El curso debería haberse modificado correctamente.");
            Assert.That(cursoModificado, Is.Not.Null, "El curso modificado no debería ser nulo.");

            Assert.Multiple(() =>
            {
                Assert.That(cursoModificado.nombreCurso, Is.EqualTo("Curso actualizado"));
                Assert.That(cursoModificado.descripcion, Is.EqualTo("Descripción modificada"));
                Assert.That(cursoModificado.nivel, Is.EqualTo(2));
                Assert.That(cursoModificado.cupoMaximo, Is.EqualTo(25));
                Assert.That(cursoModificado.estado, Is.EqualTo("Inactivo"));
                Assert.That(cursoModificado.fechaInicio, Is.EqualTo(cursoDTO.fechaInicio));
                Assert.That(cursoModificado.fechaTermino, Is.EqualTo(cursoDTO.fechaTermino));
                Assert.That(cursoModificado.idIdioma, Is.EqualTo(2));
                Assert.That(cursoModificado.idProfesor, Is.EqualTo(3));
            });

            mockDAO.Verify(dao => dao.ModificarCurso(It.IsAny<Curso>()), Times.Once, "El método ModificarCurso debería haberse invocado una vez.");
        }

        [Test]
        public void EditarCurso_DevuelveFalse_SiNoExisteElCurso()
        {
            // Arrange: Simular que el curso no existe
            mockDAO.Setup(dao => dao.ObtenerCursoPorID(999))
                   .Returns((Curso)null);

            var cursoDTO = new CursoDTO
            {
                idCurso = 999,  // ID inexistente
                nombreCurso = "Curso inexistente",
                descripcion = "No debería modificar",
                nivel = 1,
                cupoMaximo = 20,
                estado = "Inactivo",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(30),
                idIdioma = 1,
                idProfesor = 1
            };

            // Act
            var resultado = service.ModificarCurso(cursoDTO);

            // Assert
            Assert.That(resultado, Is.False, "El resultado debería ser false si el curso no existe.");
            mockDAO.Verify(dao => dao.ModificarCurso(It.IsAny<Curso>()), Times.Never, "No debería llamar a ModificarCurso si el curso no existe.");
        }

        [Test]
        public void ObtenerCursoPorID()
        { 
            var curso = new Curso
            {
                idCurso = 1,
                nombreCurso = "Matemáticas",
                descripcion = "Curso avanzado",
                nivel = 2,
                cupoMaximo = 20,
                estado = "Activo",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddMonths(1),
                idIdioma = 1,
                idProfesor = 10
            };

            mockDAO.Setup(dao => dao.ObtenerCursoPorID(1)).Returns(curso);
             
            var resultado = service.ObtenerCursoPorID(1);
             
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.idCurso, Is.EqualTo(1));
            Assert.That(resultado.nombreCurso, Is.EqualTo("Matemáticas"));
            Assert.That(resultado.nivel, Is.EqualTo(2));
            Assert.That(resultado.cupoMaximo, Is.EqualTo(20));
            Assert.That(resultado.estado, Is.EqualTo("Activo"));
            Assert.That(resultado.idIdioma, Is.EqualTo(1));
            Assert.That(resultado.idProfesor, Is.EqualTo(10));
        }

        [Test]
        public void ObtenerCursoPorID_DevuelveNull_CuandoNoExiste()
        {

            mockDAO.Setup(dao => dao.ObtenerCursoPorID(999)).Returns((Curso)null); 

            var resultado = service.ObtenerCursoPorID(999);
             
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursoPorID_ManejaExcepcion_EntityException()
        {
            // Arrange
            mockDAO.Setup(dao => dao.ObtenerCursoPorID(1))
                   .Throws(new System.Data.Entity.Core.EntityException("Error de conexión"));

            // Act
            var resultado = service.ObtenerCursoPorID(1);

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursoPorID_ManejaExcepcion_SqlException()
        {
            // Arrange
            mockDAO.Setup(dao => dao.ObtenerCursoPorID(1))
                   .Throws(new Exception());

            // Act
            var resultado = service.ObtenerCursoPorID(1);

            // Assert
            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_CursosEncontrados_DevuelveDTOs()
        {
            var cursos = new[]
            {
                new Curso { idCurso = 1, nombreCurso = "Español Básico", nivel = 1, descripcion = "Curso básico",
                            fechaInicio = DateTime.Now, fechaTermino = DateTime.Now.AddMonths(1),
                            cupoMaximo = 20, estado = "Activo", idIdioma = 1, idProfesor = 2 },
                new Curso { idCurso = 2, nombreCurso = "Español Intermedio", nivel = 2, descripcion = "Curso intermedio",
                            fechaInicio = DateTime.Now, fechaTermino = DateTime.Now.AddMonths(1),
                            cupoMaximo = 15, estado = "Activo", idIdioma = 1, idProfesor = 3 }
            };

            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreIdioma("Español"))
                   .Returns(cursos);

            var resultado = service.ObtenerCursosPorNombreIdioma("Español");

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Length, Is.EqualTo(2));
            Assert.That(resultado[0].nombreCurso, Is.EqualTo("Español Básico"));
            Assert.That(resultado[1].nombreCurso, Is.EqualTo("Español Intermedio"));
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_SinCursos_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreIdioma("Inexistente"))
                   .Returns((Curso[])null);

            var resultado = service.ObtenerCursosPorNombreIdioma("Inexistente");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_EntityException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreIdioma("Error"))
                   .Throws(new EntityException("Error de EF"));

            var resultado = service.ObtenerCursosPorNombreIdioma("Error");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_SqlException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreIdioma("ErrorSQL"))
                   .Throws(new Exception());

            var resultado = service.ObtenerCursosPorNombreIdioma("ErrorSQL");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_Exception_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreIdioma("ErrorGeneral"))
                   .Throws(new Exception("Error inesperado"));

            var resultado = service.ObtenerCursosPorNombreIdioma("ErrorGeneral");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNivel_CursosEncontrados_DevuelveDTOs()
        {
            var cursos = new[]
            {
                new Curso { idCurso = 1, nombreCurso = "Matemáticas Básicas", nivel = 1, descripcion = "Curso básico",
                            fechaInicio = DateTime.Now, fechaTermino = DateTime.Now.AddMonths(1),
                            cupoMaximo = 20, estado = "Activo", idIdioma = 1, idProfesor = 2 }
            };

            mockDAO.Setup(dao => dao.ObtenerCursosPorNivel(1))
                   .Returns(cursos);

            var resultado = service.ObtenerCursosPorNivel(1);

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Length, Is.EqualTo(1));
            Assert.That(resultado[0].nombreCurso, Is.EqualTo("Matemáticas Básicas"));
        }

        [Test]
        public void ObtenerCursosPorNivel_SinCursos_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNivel(10))
                   .Returns((Curso[])null);

            var resultado = service.ObtenerCursosPorNivel(10);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNivel_EntityException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNivel(2))
                   .Throws(new EntityException("Error de EF"));

            var resultado = service.ObtenerCursosPorNivel(2);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNivel_SqlException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNivel(3))
                   .Throws(new Exception());

            var resultado = service.ObtenerCursosPorNivel(3);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNivel_Exception_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNivel(4))
                   .Throws(new Exception("Error inesperado"));

            var resultado = service.ObtenerCursosPorNivel(4);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreProfesor_CursosEncontrados_DevuelveDTOs()
        {
            var cursos = new[]
            {
                new Curso { idCurso = 1, nombreCurso = "Inglés Básico", nivel = 1, descripcion = "Curso básico",
                            fechaInicio = DateTime.Now, fechaTermino = DateTime.Now.AddMonths(1),
                            cupoMaximo = 20, estado = "Activo", idIdioma = 1, idProfesor = 2 }
            };

            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreProfesor("Juan Perez"))
                   .Returns(cursos);

            var resultado = service.ObtenerCursosPorNombreProfesor("Juan Perez");

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Length, Is.EqualTo(1));
            Assert.That(resultado[0].nombreCurso, Is.EqualTo("Inglés Básico"));
        }

        [Test]
        public void ObtenerCursosPorNombreProfesor_SinCursos_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreProfesor("Inexistente"))
                   .Returns((Curso[])null);

            var resultado = service.ObtenerCursosPorNombreProfesor("Inexistente");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreProfesor_EntityException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreProfesor("Error"))
                   .Throws(new EntityException("Error de EF"));

            var resultado = service.ObtenerCursosPorNombreProfesor("Error");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreProfesor_SqlException_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreProfesor("ErrorSQL"))
                   .Throws(new Exception());

            var resultado = service.ObtenerCursosPorNombreProfesor("ErrorSQL");

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerCursosPorNombreProfesor_Exception_DevuelveNull()
        {
            mockDAO.Setup(dao => dao.ObtenerCursosPorNombreProfesor("ErrorGeneral"))
                   .Throws(new Exception("Error inesperado"));

            var resultado = service.ObtenerCursosPorNombreProfesor("ErrorGeneral");

            Assert.That(resultado, Is.Null);
        }


    }
}