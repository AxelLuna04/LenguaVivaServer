using NUnit.Framework;
using ServicioContrato;
using AccesoDatos.DAO;
using AccesoDatos;
using AccesoDatos.Interfaces;
using System.Linq;
using System.Data.Entity;
using System.Transactions;

namespace Pruebas.PruebasIntegracion
{
    [TestFixture]
    public class PruebasIdiomasIntegracion
    {
        private LenguaVivaDBEntities contexto;
        private ImplementacionServicio servicio;
        private TransactionScope transaccion;

        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config"));
            contexto = new LenguaVivaDBEntities();
            servicio = new ImplementacionServicio();

            // Iniciar una transacción para asegurar que los datos se reviertan después de cada prueba
            transaccion = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        [TearDown]
        public void Cleanup()
        {
            // Limpieza después de cada prueba
            contexto.Dispose();
            transaccion.Dispose();
        }

        [Test]
        public void CargarIdiomas_DevuelveIdiomasCorrectamente()
        {
            // Arrange: Insertar idiomas reales en la base de datos
            var idioma1 = new Idioma { nombreIdioma = "Español" };
            var idioma2 = new Idioma { nombreIdioma = "Inglés" };

            contexto.Idioma.AddRange(new[] { idioma1, idioma2 });
            contexto.SaveChanges();

            // Act
            var resultado = servicio.CargarIdiomas()
                .Where(i => i.nombreIdioma == "Español" || i.nombreIdioma == "Inglés")
                .ToArray();

            // Assert
            Assert.That(resultado.Length, Is.EqualTo(2));

            Assert.Multiple(() =>
            {
                Assert.That(resultado[0].nombreIdioma, Is.EqualTo("Español"));
                Assert.That(resultado[1].nombreIdioma, Is.EqualTo("Inglés"));
            });
        }

    }
}
