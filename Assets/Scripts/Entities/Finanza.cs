#nullable enable

using System;

namespace TacticalEleven.Scripts
{
    public class Finanza
    {
        public int IdFinanza { get; set; }
        public int IdEquipo { get; set; }
        public int IdManager { get; set; }
        public string? Temporada { get; set; }
        public int IdConcepto { get; set; }
        public int Tipo { get; set; }
        public double Cantidad { get; set; }
        public DateTime Fecha { get; set; }

        // Constructor sin parámetros
        public Finanza()
        {
        }

        // Constructor con parámetros
        public Finanza(int idFinanza, int idEquipo, int idManager, string temporada, int idConcepto, int tipo, double cantidad, DateTime fecha)
        {
            IdFinanza = idFinanza;
            IdEquipo = idEquipo;
            IdManager = idManager;
            Temporada = temporada;
            IdConcepto = idConcepto;
            Tipo = tipo;
            Cantidad = cantidad;
            Fecha = fecha;
        }

        // Constructor con parámetros sin ID
        public Finanza(int idEquipo, int idManager, string temporada, int idConcepto, int tipo, double cantidad, DateTime fecha)
        {
            IdEquipo = idEquipo;
            IdManager = idManager;
            Temporada = temporada;
            IdConcepto = idConcepto;
            Tipo = tipo;
            Cantidad = cantidad;
            Fecha = fecha;
        }

        // Método ToString para mostrar los datos de la clase
        public override string ToString()
        {
            return $"ID: {IdFinanza}, Equipo: {IdEquipo}, Temporada: {Temporada}, Concepto: {IdConcepto}, Tipo: {Tipo}, Cantidad: {Cantidad:C}, Fecha: {Fecha:yyyy-MM-dd}";
        }
    }
}