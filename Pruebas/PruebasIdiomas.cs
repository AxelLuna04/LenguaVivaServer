using NUnit.Framework;
using ServicioContrato;
using AccesoDatos.DAO;
using AccesoDatos;
using Moq;
using System.Data.SqlClient;
using System.Data.Entity;
using AccesoDatos.Interfaces;

namespace PruebasUnitarias
{
    [TestFixture]
    public class ImplementacionServicioTests
    {
        private Mock<IIdiomaDAO> mockDAO;
        private ImplementacionServicio service;

        [SetUp]
        public void Setup()
        {
            mockDAO = new Mock<IIdiomaDAO>();
            service = new ImplementacionServicio(mockDAO.Object, null);
        }

        [Test]
        public void CargarIdiomas()
        { 
            var idiomasMock = new[]
            {
            new Idioma { idIdioma = 1, nombreIdioma = "Español" },
            new Idioma { idIdioma = 2, nombreIdioma = "Inglés" }
            };

            mockDAO.Setup(dao => dao.CargarIdiomas()).Returns(idiomasMock); 

            var resultado = service.CargarIdiomas();

            Assert.That(resultado.Length, Is.EqualTo(2));
            Assert.That(resultado[0].nombreIdioma, Is.EqualTo("Español"));
            Assert.That(resultado[1].nombreIdioma, Is.EqualTo("Inglés"));
        }

        [Test]
        public void RegistrarIdioma()
        { 

#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            Idioma idiomaGuardado = null;

            mockDAO.Setup(dao => dao.CrearIdioma(It.IsAny<Idioma>()))
                   .Callback<Idioma>(idioma => idiomaGuardado = idioma); 

            service.RegistrarIdioma("Alemán");

            Assert.That(idiomaGuardado, Is.Not.Null);
            Assert.That(idiomaGuardado.nombreIdioma, Is.EqualTo("Alemán"));
        }

    }
}