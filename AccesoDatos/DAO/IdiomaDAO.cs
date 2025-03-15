using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class IdiomaDAO
    {
        public static void CrearIdioma(Idioma idioma)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Idioma.Add(idioma);
                contexto.SaveChanges();
            }
        }

        public static Idioma ObtenerIdiomaPorNombre(String nombreIdioma)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Idioma.FirstOrDefault(i => i.nombreIdioma == nombreIdioma);
            }
        }

        public static Idioma[] ObtenerIdiomas()
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Idioma.ToArray();
            }
        }
    }
}
