using System;
using System.Collections.Generic;

namespace TacticalEleven.Scripts
{
    public class Partido
    {
        public static List<int> idsPartidos = new List<int>();

        // Atributos
        public int IdPartido { get; set; }
        public int IdCompeticion { get; set; }
        public int? Jornada { get; set; }
        public int? Ronda { get; set; }
        public int? PartidoVuelta { get; set; }
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
        public DateTime FechaPartido { get; set; }
        public string Estado { get; set; }
        public int? Asistencia { get; set; }

        // Atributos extra
        public string NombreEquipoLocal { get; set; }
        public string NombreEquipoVisitante { get; set; }

        // Constructor por defecto
        public Partido()
        {

        }

        // Constructor con parámetros
        public Partido(int idPartido, int idCompeticion, int? jornada, int? ronda, int? partidoVuelta, int idEquipoLocal, int idEquipoVisitante,
                       int? golesLocal, int? golesVisitante, DateTime fechaPartido, string estado, int? asistencia)
        {
            IdPartido = idPartido;
            IdCompeticion = idCompeticion;
            Jornada = jornada;
            Ronda = ronda;
            PartidoVuelta = partidoVuelta;
            IdEquipoLocal = idEquipoLocal;
            IdEquipoVisitante = idEquipoVisitante;
            GolesLocal = golesLocal;
            GolesVisitante = golesVisitante;
            FechaPartido = fechaPartido;
            Estado = estado;
            Asistencia = asistencia;
        }

        // Constructor con parámetros sin ID
        public Partido(int idCompeticion, int? jornada, int? ronda, int? partidoVuelta, int idEquipoLocal, int idEquipoVisitante,
                       int? golesLocal, int? golesVisitante, DateTime fechaPartido, string estado, int? asistencia)
        {
            IdCompeticion = idCompeticion;
            Jornada = jornada;
            Ronda = ronda;
            PartidoVuelta = partidoVuelta;
            IdEquipoLocal = idEquipoLocal;
            IdEquipoVisitante = idEquipoVisitante;
            GolesLocal = golesLocal;
            GolesVisitante = golesVisitante;
            FechaPartido = fechaPartido;
            Estado = estado;
            Asistencia = asistencia;
        }

        // Sobrescritura del método ToString()
        public override string ToString()
        {
            return $"Partido {IdPartido}: Equipo {IdEquipoLocal} vs Equipo {IdEquipoVisitante} | " +
                   $"Resultado: {GolesLocal}-{GolesVisitante} | Fecha: {FechaPartido.ToString("yyyy-MM-dd HH:mm")} | " +
                   $"Estado: {Estado} | Asistencia: {Asistencia} | Jornada: {Jornada} | Ronda: {Ronda}";
        }
    }
}