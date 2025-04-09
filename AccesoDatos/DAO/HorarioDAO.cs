using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class HorarioDAO
    {
        public Horario ObtenerHorario(int idCurso)
        {
            using(var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Horario
                    .FirstOrDefault(h => h.idCurso == idCurso);
            }
        }
    }
}
