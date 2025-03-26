using NUnit.Framework;
using ServicioContrato;
using AccesoDatos;
using System;
using System.Linq;
using System.Transactions;
using AccesoDatos.DAO;

namespace Pruebas.PruebasIntegracion
{
    [TestFixture]
    public class PruebasCursosIntegracion
    {
        private LenguaVivaDBEntities contexto;
        private ImplementacionServicio service;
        private TransactionScope transaccion;

        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config"));
            contexto = new LenguaVivaDBEntities();  
            service = new ImplementacionServicio();

            // Iniciar una transacción para asegurar que los datos se reviertan después de cada prueba
            transaccion = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        [TearDown]
        public void TearDown()
        {
            // Finalizar la transacción después de cada prueba para revertir cambios
            contexto.Dispose();
            transaccion.Dispose();
        }

        [Test]
        public void RegistrarCurso_CreaCursoCorrectamente()
        {
            // Arrange
            var cursoDTO = new CursoDTO
            {
                nombreCurso = "Curso de integración",
                nivel = 1,
                descripcion = "Prueba de integración",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(10),
                cupoMaximo = 30,
                idIdioma = 1,
                idProfesor = 1
            };

            // Act
            service.RegistrarCurso(cursoDTO);

            // Assert

            var cursoEnBD =  contexto.Curso.FirstOrDefault(c => c.nombreCurso == "Curso de integración");

            Assert.That(cursoEnBD, Is.Not.Null, "El curso debería existir en la base de datos.");
            Assert.Multiple(() =>
            {
                Assert.That(cursoEnBD.descripcion, Is.EqualTo("Prueba de integración"));
                Assert.That(cursoEnBD.cupoMaximo, Is.EqualTo(30));
                Assert.That(cursoEnBD.nivel, Is.EqualTo(1));
            });
        }

        [Test]
        public void EditarCurso_ModificaCursoCorrectamente()
        {
            // Arrange
            var curso = new Curso
            {
                nombreCurso = "Curso original",
                nivel = 1,
                descripcion = "Curso de prueba",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(30),
                cupoMaximo = 20,
                estado = "Activo",
                idIdioma = 1,
                idProfesor = 1
            };

            contexto.Curso.Add(curso);
            contexto.SaveChanges();

            var cursoDTO = new CursoDTO
            {
                idCurso = curso.idCurso,
                nombreCurso = "Curso actualizado",
                descripcion = "Descripción modificada",
                nivel = 2,
                cupoMaximo = 25,
                estado = "Inactivo",
                fechaInicio = (DateTime)curso.fechaInicio,
                fechaTermino = (DateTime)curso.fechaTermino,
                idIdioma = 1,
                idProfesor = 3
            };

            // Act
            var resultado = service.ModificarCurso(cursoDTO);
            contexto.Entry(curso).Reload();
            // Assert
            var cursoEnBD = contexto.Curso.FirstOrDefault(c => c.idCurso == curso.idCurso);

            Assert.That(resultado, Is.True, "El curso debería haberse modificado correctamente.");
            Assert.That(cursoEnBD, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(cursoEnBD.idCurso, Is.EqualTo(cursoDTO.idCurso));
                Assert.That(cursoEnBD.nombreCurso, Is.EqualTo("Curso actualizado"));
                Assert.That(cursoEnBD.descripcion, Is.EqualTo("Descripción modificada"));
                Assert.That(cursoEnBD.nivel, Is.EqualTo(2));
                Assert.That(cursoEnBD.cupoMaximo, Is.EqualTo(25));
                Assert.That(cursoEnBD.estado, Is.EqualTo("Inactivo"));
            });
        }

        [Test]
        public void ObtenerCursoPorID_DevuelveCursoCorrecto()
        {
            // Arrange
            var curso = new Curso
            {
                nombreCurso = "Curso de consulta",
                nivel = 1,
                descripcion = "Consulta de prueba",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddDays(15),
                cupoMaximo = 15,
                estado = "Activo",
                idIdioma = 1,
                idProfesor = 1
            };

            contexto.Curso.Add(curso);
            contexto.SaveChanges();

            // Act
            var resultado = service.ObtenerCursoPorID(curso.idCurso);

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.idCurso, Is.EqualTo(curso.idCurso));
            Assert.That(resultado.nombreCurso, Is.EqualTo("Curso de consulta"));
        }

        [Test]
        public void ObtenerCursoPorID_DevuelveNull_CuandoNoExiste()
        {
            // Act
            var resultado = service.ObtenerCursoPorID(9999); // ID inexistente

            // Assert
            Assert.That(resultado, Is.Null, "El resultado debería ser null si el curso no existe.");
        }

        [Test]
        public void ObtenerCursosPorNombreIdioma_DevuelveCursosCorrectamente()
        {
            // Arrange
            var curso1 = new Curso
            {
                nombreCurso = "Inglés Básico",
                nivel = 1,
                descripcion = "Curso básico",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddMonths(1),
                cupoMaximo = 20,
                estado = "Activo",
                idIdioma = 1,
                idProfesor = 1
            };

            var curso2 = new Curso
            {
                nombreCurso = "Inglés Intermedio",
                nivel = 2,
                descripcion = "Curso intermedio",
                fechaInicio = DateTime.Now,
                fechaTermino = DateTime.Now.AddMonths(1),
                cupoMaximo = 15,
                estado = "Activo",
                idIdioma = 1,
                idProfesor = 2
            };

            contexto.Curso.AddRange(new[] { curso1, curso2 });
            contexto.SaveChanges();

            // Act
            var resultado = service.ObtenerCursosPorNombreIdioma("Ingles");

            // Assert
            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.Length, Is.AtLeast(2));
        }
    }
}

