namespace TacticalEleven.Scripts
{
    public class HistorialJugador
    {
        public int IdHistorial { get; set; }
        public string Temporada { get; set; }
        public int IdJugadorOro { get; set; }
        public int IdJugadorPlata { get; set; }
        public int IdJugadorBronce { get; set; }

        // Otros atributos
        public string NombreJugadorOro { get; set; }
        public string NombreJugadorPlata { get; set; }
        public string NombreJugadorBronce { get; set; }
        public int EquipoJugadorOro { get; set; }
        public int EquipoJugadorPlata { get; set; }
        public int EquipoJugadorBronce { get; set; }


        // Constructor vacío
        public HistorialJugador() { }

        // Constructor con parámetros
        public HistorialJugador(int idHistorial, string temporada, int idJugadorOro, int idJugadorPlata, int idJugadorBronce)
        {
            IdHistorial = idHistorial;
            Temporada = temporada;
            IdJugadorOro = idJugadorOro;
            IdJugadorPlata = idJugadorPlata;
            IdJugadorBronce = idJugadorBronce;
        }

        // Constructor con parámetros sin ID
        public HistorialJugador(string temporada, int idJugadorOro, int idJugadorPlata, int idJugadorBronce)
        {
            Temporada = temporada;
            IdJugadorOro = idJugadorOro;
            IdJugadorPlata = idJugadorPlata;
            IdJugadorBronce = idJugadorBronce;
        }
    }
}