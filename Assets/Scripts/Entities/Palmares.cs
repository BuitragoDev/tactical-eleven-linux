#nullable enable

namespace TacticalEleven.Scripts
{
    public class Palmares
    {
        public int IdPalmares { get; set; }
        public int IdEquipo { get; set; }
        public int IdCompeticion { get; set; }
        public int Titulos { get; set; }

        // Atributos extra
        public string? NombreEquipo { get; set; }
        public string? NombreCompeticion { get; set; }

        // Constructor por defecto
        public Palmares()
        {
        }

        // Constructor con parámetros
        public Palmares(int idPalmares, int idEquipo, int idCompeticion, string temporada, int titulos)
        {
            IdPalmares = idPalmares;
            IdEquipo = idEquipo;
            IdCompeticion = idCompeticion;
            Titulos = titulos;
        }

        // Constructor con parámetros sin ID
        public Palmares(int idEquipo, int idCompeticion, string temporada, int titulos)
        {
            IdEquipo = idEquipo;
            IdCompeticion = idCompeticion;
            Titulos = titulos;
        }

        // Método ToString
        public override string ToString()
        {
            return $"IdPalmares: {IdPalmares}, IdEquipo: {IdEquipo}, IdCompeticion: {IdCompeticion}, Titulos: {Titulos}";
        }
    }
}