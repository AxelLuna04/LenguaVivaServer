using AccesoDatos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class IdiomaDAO : IIdiomaDAO
    {
        public   void CrearIdioma(Idioma idioma)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                contexto.Idioma.Add(idioma);
                contexto.SaveChanges();
            }
        }

        public   Idioma ObtenerIdiomaPorNombre(String nombreIdioma)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Idioma.FirstOrDefault(i => i.nombreIdioma == nombreIdioma);
            }
        }

        public   Idioma[] ObtenerIdiomas()
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                List<Idioma> respuesta = new List<Idioma>();

                foreach (Idioma idioma in contexto.Idioma)
                {
                    respuesta.Add(idioma);
                }


                return respuesta.ToArray();
            }
        }

        public  Idioma ObtenerIdiomaPorID(int id)
        {
            using (var contexto = new LenguaVivaDBEntities())
            { 
                return contexto.Idioma.Find(id);
            }
        }

        public Idioma[] CargarIdiomas()
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Idioma.ToArray();
            }
        }
    }
}