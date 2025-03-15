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
            // Simulación de base de datos
            Dictionary<string, string> usuarios = new Dictionary<string, string>
            {
                { "Axel", "12345" },
                { "Juan", "qwerty" },
                { "Maria", "abc123" }
            };

            return usuarios.ContainsKey(usuario) && usuarios[usuario] == contrasenia;

        }
    }
}
