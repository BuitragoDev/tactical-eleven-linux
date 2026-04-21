#nullable enable

namespace TacticalEleven.Scripts
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public int Orden { get; set; }
        public string? Fecha { get; set; }
        public int Capital { get; set; }
        public int CapitalRestante { get; set; }
        public int Semanas { get; set; }
        public int SemanasRestantes { get; set; }
        public int TasaInteres { get; set; }
        public int PagoSemanal { get; set; }
        public int IdManager { get; set; }
        public int IdEquipo { get; set; }

        // Constructor vacío
        public Prestamo() { }

        // Constructor con parámetros
        public Prestamo(int idPrestamo, int orden, string fecha, int capital, int capitalRestante, int semanas,
                        int semanasRestantes, int tasaInteres, int pagoSemanal, int idManager, int idEquipo)
        {
            IdPrestamo = idPrestamo;
            Orden = orden;
            Fecha = fecha;
            Capital = capital;
            CapitalRestante = capitalRestante;
            Semanas = semanas;
            SemanasRestantes = semanasRestantes;
            TasaInteres = tasaInteres;
            PagoSemanal = pagoSemanal;
            IdManager = idManager;
            IdEquipo = idEquipo;
        }

        // Constructor con parámetros sin ID
        public Prestamo(string fecha, int orden, int capital, int capitalRestante, int semanas,
                        int semanasRestantes, int tasaInteres, int pagoSemanal, int idManager, int idEquipo)
        {
            Orden = orden;
            Fecha = fecha;
            Capital = capital;
            CapitalRestante = capitalRestante;
            Semanas = semanas;
            SemanasRestantes = semanasRestantes;
            TasaInteres = tasaInteres;
            PagoSemanal = pagoSemanal;
            IdManager = idManager;
            IdEquipo = idEquipo;
        }

        // Método ToString para representar la información
        public override string ToString()
        {
            return $"Prestamo[ID: {IdPrestamo}, Orden: {Orden}, Fecha: {Fecha}, Capital: {Capital}, " +
                   $"Capital Restante: {CapitalRestante}, Semanas: {Semanas}, " +
                   $"Semanas Restantes: {SemanasRestantes}, Tasa Interés: {TasaInteres}, " +
                   $"Pago Semanal: {PagoSemanal}, ID Manager: {IdManager}, ID Equipo: {IdEquipo}]";
        }
    }
}