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
                List<Idioma> respuesta = new List<Idioma>();

                foreach (Idioma idioma in contexto.Idioma)
                {
                    respuesta.Add(idioma);
                }


                return respuesta.ToArray();
            }
        }

        public static Idioma ObtenerIdiomaPorID(int id)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                return contexto.Idioma.Find(id);
            }
        }

        public static Idioma[] CargarIdiomas()
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Idioma.ToArray();
            }
        }
    }
}