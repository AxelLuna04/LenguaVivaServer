using AccesoDatos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class PagoDAO : IPagoDAO
    {
        public void RegistrarPago(Pago pago)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                contexto.Database.Log = Console.WriteLine;
                contexto.Pago.Add(pago);
                contexto.SaveChanges();
            }
        }

        public Pago ObetenerPago(int idInscripcion)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var pago = contexto.Pago.FirstOrDefault(p => p.idInscripcion == idInscripcion);
                return pago;
            }
        }
    }
}
