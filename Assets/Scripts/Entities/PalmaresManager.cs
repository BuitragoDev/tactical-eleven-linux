#nullable enable

namespace TacticalEleven.Scripts
{
    public class PalmaresManager
    {
        public int IdPalmares { get; set; }
        public int IdEquipo { get; set; }
        public int IdCompeticion { get; set; }
        public string? Temporada { get; set; }

        // Constructor por defecto
        public PalmaresManager()
        {
        }

        // Constructor con parámetros
        public PalmaresManager(int idPalmares, int idEquipo, int idCompeticion, string temporada)
        {
            IdPalmares = idPalmares;
            IdEquipo = idEquipo;
            IdCompeticion = idCompeticion;
            Temporada = temporada;
        }

        // Constructor con parámetros sin ID
        public PalmaresManager(int idEquipo, int idCompeticion, string temporada)
        {
            IdEquipo = idEquipo;
            IdCompeticion = idCompeticion;
            Temporada = temporada;
        }

        // Método ToString
        public override string ToString()
        {
            return $"IdPalmares: {IdPalmares}, IdEquipo: {IdEquipo}, IdCompeticion: {IdCompeticion}, Temporada: {Temporada}";
        }
    }
}