using System;

namespace TacticalEleven.Scripts
{
    public class Remodelacion
    {
        public int IdRemodelacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int TipoRemodelacion { get; set; }
        public int IdEquipo { get; set; }
        public int IdManager { get; set; }

        // Constructor sin parámetros
        public Remodelacion()
        {
        }

        // Constructor con parámetros
        public Remodelacion(int idRemodelacion, DateTime fechaInicio, DateTime fechaFinal, int tipoRemodelacion, int idEquipo, int idManager)
        {
            IdRemodelacion = idRemodelacion;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            TipoRemodelacion = tipoRemodelacion;
            IdEquipo = idEquipo;
            IdManager = idManager;
        }

        // Constructor parcial (sin IdRemodelacion)
        public Remodelacion(DateTime fechaInicio, DateTime fechaFinal, int tipoRemodelacion, int idEquipo, int idManager)
        {
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            TipoRemodelacion = tipoRemodelacion;
            IdEquipo = idEquipo;
            IdManager = idManager;
        }

        // Método ToString
        public override string ToString()
        {
            return $"Remodelación [Id: {IdRemodelacion}, Inicio: {FechaInicio.ToShortDateString()}, Final: {FechaFinal.ToShortDateString()}, Tipo: {TipoRemodelacion}, Equipo: {IdEquipo}, Manager: {IdManager}]";
        }
    }
}