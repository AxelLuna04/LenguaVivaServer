using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class UsuarioDAO
    {
        public static void AgregarAdministrador(string nombreUsuario, string contrasenia, string nombre, string apellidos, string direccion, string email)
        {
            using (var contextoBD = new LenguaVivaDBEntities())
            {
                    var nuevoAdministrador = new Administrador()
                    {
                        nombreUsuario = nombreUsuario,
                        contrasenia = contrasenia,
                        nombre = nombre,
                        apellidos = apellidos,
                        direccion = direccion,
                        email = email
                    };

                    contextoBD.Administrador.Add(nuevoAdministrador);
                    int registrosAfectados = contextoBD.SaveChanges();

                    if (registrosAfectados == 0)
                    {
                        Console.WriteLine("No se guardó ningún registro en la base de datos.");
                    }
                    else
                    {
                        Console.WriteLine($"Se guardaron {registrosAfectados} registros correctamente.");
                    }

            }
        }



    }
}
