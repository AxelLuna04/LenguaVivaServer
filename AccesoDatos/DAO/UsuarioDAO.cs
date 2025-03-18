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
        public static void CrearAdministrador(Administrador administrador)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Administrador.Add(administrador);
                contexto.SaveChanges();
            }
        }



    }
}
