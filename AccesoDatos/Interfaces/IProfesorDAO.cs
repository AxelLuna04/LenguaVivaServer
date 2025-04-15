using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaces
{
    public interface IProfesorDAO
    {
        void CrearProfesor(Profesor profesor);
        void ModificarProfesor(Profesor profesor);
        void EliminarProfesor(Profesor profesor);
        Profesor[] ObtenerProfesores();
        Profesor ObtenerProfesorPorId(int id);
        Profesor ObtenerProfesorPorUsuario(String usuario);
        Profesor[] ObtenerProfesoresPorNombre(string nombre);
        Profesor[] ObtenerTodos(); // <-- aquí debe coincidir
    }
}
