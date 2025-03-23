using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaces
{
    public interface IIdiomaDAO
    {

        void CrearIdioma(Idioma idioma);
        Idioma ObtenerIdiomaPorNombre(String nombreIdioma);
        Idioma[] ObtenerIdiomas();
        Idioma ObtenerIdiomaPorID(int id);
        Idioma[] CargarIdiomas();
    }
}
