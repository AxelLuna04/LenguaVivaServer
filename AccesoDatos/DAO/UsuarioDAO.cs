using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class UsuarioDAO
    {

        private string baseServer = "Server=JARLOCHO\\SQLEXPRESS;Database=LenguaVivaDB;User Id=AdminLV;Password=AdminLV;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

        public bool RegistrarAdministrador(string nombreUsuario, string contrasenia, string nombre, string apellidos, string direccion, string email)
        {
            using (SqlConnection conexion = new SqlConnection(baseServer)){

                try
                {
                    conexion.Open();
                    string query = "INSERT INTO Administrador (nombreUsuario, contrasenia, nombre, apellidos, direccion, email) " +
                                   "VALUES (@nombreUsuario, @contrasenia, @nombre, @apellidos, @direccion, @email)";

                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        comando.Parameters.AddWithValue("@contrasenia", contrasenia);
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@apellidos", apellidos);
                        comando.Parameters.AddWithValue("@direccion", direccion);
                        comando.Parameters.AddWithValue("@email", email);

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en la base de datos: " + ex.Message);
                    return false;
                }


            }

        }


    }
}
