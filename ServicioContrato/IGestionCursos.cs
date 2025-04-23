using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using AccesoDatos.DAO;

namespace ServicioContrato
{
    [ServiceContract]
    public interface IGestionCursos
    {
        [OperationContract]
        CursoDTO[] ObtenerCursosPorNombre(String nombre);

        [OperationContract]
        CursoDTO[] ObtenerCursosPorNombreIdioma(String nombreIdioma);

        [OperationContract]
        CursoDTO[] ObtenerCursosPorNivel(int nivel);

        [OperationContract]
        CursoDTO[] ObtenerCursosPorNombreProfesor(String nombreProfesor);

        [OperationContract]
        bool RegistrarCurso(CursoDTO cursoDTO);

        [OperationContract]
        ProfesorDTO[] ObtenerProfesoresPorNombre(String nombre);

        [OperationContract]
        ProfesorDTO ObtenerProfesorPorID(int idProfesor);

        [OperationContract]
        IdiomaDTO[] ObtenerIdiomas();

        [OperationContract]
        IdiomaDTO ObtenerIdiomaPorID(int idIdioma);

        [OperationContract]
        CursoDTO ObtenerCursoPorID(int idCurso);

        [OperationContract]
        bool ModificarCurso(CursoDTO cursoDTO);

        [OperationContract]
        bool EliminarCurso(int idCurso);

        [OperationContract]
        bool TieneAlumnosInscritos(int idCurso);

        [OperationContract]
        bool AsignarHorario(HorarioDTO horario);

        [OperationContract]
        HorarioDTO[] ObtenerHorariosPorCurso(int idCurso);

        [OperationContract]
        bool SubirListaAlumnos(int idCurso, byte[] archivo);

        [OperationContract]
        int ObtenerIngresosDeCurso(int idCurso);

        [OperationContract]
        int ObtenerNumeroAlumnosInscritosACurso(int idCurso);

    }
    [DataContract]
    public class HorarioDTO
    {
        [DataMember]
        public int idHorario { get; set; }

        [DataMember]
        public int idCurso { get; set; }

        [DataMember]
        public string diaSemana { get; set; }

        [DataMember]
        public TimeSpan horaInicio { get; set; }

        [DataMember]
        public TimeSpan horaTermino { get; set; }
    }

    [DataContract]
    public class CursoDTO
    {
        [DataMember]
        public int idCurso { get; set; }

        [DataMember]
        public string nombreCurso { get; set; }

        [DataMember]
        public string descripcion { get; set; }

        [DataMember]
        public int nivel { get; set; }

        [DataMember]
        public int cupoMaximo { get; set; }

        [DataMember]
        public string estado { get; set; }

        [DataMember]
        public DateTime fechaInicio { get; set; }

        [DataMember]
        public DateTime fechaTermino { get; set; }

        [DataMember]
        public byte[] listaAsistencia { get; set; }

        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public int idProfesor { get; set; }
    }

    [DataContract]
    public class CursoAlumno
    {
        [DataMember]
        public int idCurso { get; set; }

        [DataMember]
        public string nombreCurso { get; set; }

        [DataMember]
        public string nombreProfesor { get; set; }

        [DataMember]
        public string fechaInicio { get; set; }

        [DataMember]
        public string fechaTermino { get; set; }
    }


}