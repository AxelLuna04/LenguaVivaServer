using AccesoDatos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.DAO
{
    public class HorarioDAO : IHorarioDAO
    {
        public Horario[] RecuperarHorarios(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                return contexto.Horario
                    .Where(c => c.idCurso == idCurso)
                    .ToArray();
            }
        }

        public bool RegistrarHorario(Horario horario)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                try
                {
                    contexto.Horario.Add(horario);
                    contexto.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar el horario: " + ex.Message);
                    return false;
                }
            }
        }

        public Horario ObtenerHorario(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var horario = contexto.Horario.FirstOrDefault(h => h.idCurso == idCurso);
                Console.WriteLine($"Horario obtenido para curso {idCurso}: {(horario != null ? horario.dia : "null")}");
                return horario;
            }
        }

        public void EliminarHorariosPorCurso(int idCurso)
        {
            using (var contexto = new LenguaVivaDBEntities())
            {
                var horarios = contexto.Horario.Where(h => h.idCurso == idCurso);
                contexto.Horario.RemoveRange(horarios);
                contexto.SaveChanges();
            }
        }

    }
}
