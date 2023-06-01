using SQLite;
using System;

namespace PedidosRestaurante.Models
{
    public class PedidosMesasModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Id_mesm { get; set; }
        public int Id_mesa { get; set; }
        public int Id_mesx { get; set; }
        public string Mvm_mesa { get; set; }
        public string Mvm_codigo { get; set; }
        public decimal Mvm_cant { get; set; }
        public string Mvm_valor { get; set; }
        public decimal Mvm_vriva { get; set; }
        public decimal Mvm_iva { get; set; }
        public decimal Mvm_impoc { get; set; }
        public decimal Mvm_dsct2 { get; set; }
        public decimal Mvm_dsct4 { get; set; }
        public decimal Mvm_descto { get; set; }
        public decimal Mvm_neto { get; set; }
        public decimal Mvm_costo { get; set; }
        public decimal Mvm_vrvta { get; set; }
        public string Mvm_nombre { get; set; }
        public string Mvm_unidad { get; set; }
        public string Mvm_grupo { get; set; }
        public string Mvm_notas { get; set; }
        public string Mvm_hora { get; set; }
        public string Mvm_sale { get; set; }
        public int Mvm_combi { get; set; }
        public string Mvm_obra { get; set; }
        public string Mvm_puntox { get; set; }
        public string Mvm_nit { get; set; }
        public string Mvm_punto { get; set; }
        public string Mvm_clien { get; set; }
        public int Mvm_print { get; set; }
        public decimal Artivlr1_c { get; set; }
        public int Mes_estado { get; set; }
        public DateTime Mes_fecha { get; set; }
        public int Mes_tipo { get; set; }
        public decimal Lbcant { get; set; }
    }
}
