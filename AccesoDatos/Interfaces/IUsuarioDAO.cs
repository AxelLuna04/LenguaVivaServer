using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Interfaces
{
    public interface IUsuarioDAO
    {

        void CrearAdministrador(Administrador administrador);
        void CrearProfesor(Profesor profesor);
        void CrearRelacion(Idioma_Profesor relacion);
        void CrearAlumno(Alumno alumno);
        void CrearRelacionIdiomaAlumno(Idioma_Alumno relacion);
        int ObtenerTipoUsuario(string correo);
        Profesor ObtenerProfesorPorCorreo(string correo);
        Administrador ObtenerAdministradorPorCorreo(string correo);
        Alumno ObtenerAlumnoPorCorreo(string correo);
        void EliminarAdministrador(string correo);
        void EliminarProfesor(string correo);
        void EliminarAlumno(string correo);
        void ModificarAdministrador(Administrador administradorExistente);
        Profesor ObtenerProfesorPorId(int idProfesor);
        void ModificarProfesorCompleto(Profesor profesorEntity, List<int> idiomasSeleccionados);
        void ModificarAlumnoCompleto(Alumno alumnoModificado, List<Idioma_Alumno> nuevosIdiomas);
        Alumno ObtenerAlumnoPorId(int idAlumno);

    }
}
