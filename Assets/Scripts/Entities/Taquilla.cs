#nullable enable

namespace TacticalEleven.Scripts
{
    public class Taquilla
    {
        public int IdPrecio { get; set; }
        public int IdEquipo { get; set; }
        public int? PrecioEntradaGeneral { get; set; }
        public int? PrecioEntradaTribuna { get; set; }
        public int? PrecioEntradaVip { get; set; }
        public int? PrecioAbonoGeneral { get; set; }
        public int? PrecioAbonoTribuna { get; set; }
        public int? PrecioAbonoVip { get; set; }
        public int AbonosVendidos { get; set; }

        // Constructor por defecto
        public Taquilla()
        {

        }

        // Constructor con parámetros
        public Taquilla(int idPrecio, int idEquipo, int precioEntradaGeneral, int precioEntradaTribuna, int precioEntradaVip,
                        int precioAbonoGeneral, int precioAbonoTribuna, int precioAbonoVip, int abonosVendidos)
        {
            IdPrecio = idPrecio;
            IdEquipo = idEquipo;
            PrecioEntradaGeneral = precioEntradaGeneral;
            PrecioEntradaTribuna = precioEntradaTribuna;
            PrecioEntradaVip = precioEntradaVip;
            PrecioAbonoGeneral = precioAbonoGeneral;
            PrecioAbonoTribuna = precioAbonoTribuna;
            PrecioAbonoVip = precioAbonoVip;
            AbonosVendidos = abonosVendidos;
        }

        // Constructor con parámetros sin ID
        public Taquilla(int idEquipo, int precioEntradaGeneral, int precioEntradaTribuna, int precioEntradaVip,
                        int precioAbonoGeneral, int precioAbonoTribuna, int precioAbonoVip, int abonosVendidos)
        {
            IdEquipo = idEquipo;
            PrecioEntradaGeneral = precioEntradaGeneral;
            PrecioEntradaTribuna = precioEntradaTribuna;
            PrecioEntradaVip = precioEntradaVip;
            PrecioAbonoGeneral = precioAbonoGeneral;
            PrecioAbonoTribuna = precioAbonoTribuna;
            PrecioAbonoVip = precioAbonoVip;
            AbonosVendidos = abonosVendidos;
        }

        // Método ToString
        public override string ToString()
        {
            return $"IdPrecio: {IdPrecio}, IdEquipo: {IdEquipo}, " +
                   $"Precio Entrada General: {PrecioEntradaGeneral}, Precio Entrada Tribuna: {PrecioEntradaTribuna}, " +
                   $"Precio Entrada VIP: {PrecioEntradaVip}, Precio Abono General: {PrecioAbonoGeneral}, " +
                   $"Precio Abono Tribuna: {PrecioAbonoTribuna}, Precio Abono VIP: {PrecioAbonoVip}, " +
                   $"Abonos Vendidos: {AbonosVendidos}";
        }
    }
}