using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaces
{
    public interface ICursoDAO
    {
        void CrearCurso(Curso curso);
        void ModificarCurso(Curso curso);
        void EliminarCurso(int idCurso);
        Curso[] ObtenerCursos();
        Curso[] ObtenerCursosPorNombre(String nombreCurso);
        Curso ObtenerCursoPorID(int id);
        Curso[] ObtenerCursosPorNombreIdioma(String nombreIdioma);
        Curso[] ObtenerCursosPorNombreProfesor(String nombreProfesor);
        Curso[] ObtenerCursosPorNivel(int nivel);
        bool TieneAlumnosInscritos(int idCurso);

    }
}
