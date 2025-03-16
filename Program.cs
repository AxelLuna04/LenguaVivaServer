using ServicioContrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using AccesoDatos;
using AccesoDatos.DAO;

namespace LenguaVivaServidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(ImplementacionServicio)))
                {
                    host.Open();
                    Console.WriteLine("Servidor corriendo...");
                    prueba();
                    Console.ReadLine();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.ReadLine();
            }
        }

        public static void prueba()
        {
            Idioma idioma = new Idioma();
            idioma.nombreIdioma = "Ingles";
            IdiomaDAO.CrearIdioma(idioma);
            Console.WriteLine("Idioma creado");
        }
    }
}
