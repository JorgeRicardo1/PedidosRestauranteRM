using SQLite;

namespace PedidosRestaurante.Models
{
    public class MesasModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Image { get; set; }
        public int Id_mesa { get; set; }
        public string Mes_numero { get; set; }
        public int Mes_personas { get; set; }
        public string Mes_ubica { get; set; }
        public string Mes_mesero { get; set; }
        //mes_fecha datetime
        public decimal Mes_valor { get; set; }
        public int Mes_cant { get; set; }
        public int Mes_estado { get; set; }
        public int Mes_tipo { get; set; }
        public int Mes_room { get; set; }
        public string Mes_refer { get; set; }
        public string Mes_referx { get; set; }
        public decimal Mes_referv { get; set; }
        public string Mes_client { get; set; }
        public string Mes_nmes { get; set; }
        public int Res_numero { get; set; }
        public string Res_nit { get; set; }
        public string Res_nombre { get; set; }
        //res_fecha date
        //res_fechar datetime
        public string Res_tomo { get; set; }
        public string Res_notas { get; set; }
        public string Mes_image { get; set; }
        public int Pixel_left { get; set; }
        public int Pixel_top { get; set; }
        public decimal Mes_propi { get; set; }
        public string Mes_frax { get; set; }
        public bool Ischecked { get; set; }
    }
}
