using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioContrato
{
    public partial class ImplementacionServicio : IGestionUsuarios
    {
        public bool IniciarSesion(string usuario, string contrasenia)
        {
            if(usuario == "Axel" && contrasenia == "12345")
                return true;
            return false;
        }
    }
}
