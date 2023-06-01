using SQLite;

namespace PedidosRestaurante.Models
{
    public class XxxxartiModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Articodigo { get; set; }
        public string Artigrupo { get; set; }
        public string Articodi2 { get; set; }
        public string Artinomb { get; set; }
        public string Artiunidad { get; set; }
        public decimal Artxctoult { get; set; }
        public string Artivalor { get; set; }
        public int Articantidad { get; set; }

        public int Id_mesa { get; set; }
        public string Mvm_mesa { get; set; }
        public string Mvm_codigo { get; set; }

        public string Mvm_valor { get; set; }
        public string Mvm_unidad { get; set; }
        public string Mvm_grupo { get; set; }
        public string Mvm_notas { get; set; }

        public decimal Mvm_neto { get; set; }
        public int Mvm_printing { get; set; }

        public decimal Artivlr1_c { get; set; }
        public int Artiiva { get; set; }

        public int Id_mesm { get; set; }

        public int Tipo_mesa { get; set; }
        public decimal Articombi { get; set; }

        public decimal Artiped_s { get; set; }


        //DE USO PARA OBTENER LOS COMBINADOS GUARDADOS
        public int comp_id_mesm { get; set; }
        public int combinado { get; set; }
        public string Nit { get; set; }
        public decimal Librecnt { get; set; }
        public string Nombre { get; set; }
        public decimal Articant { get; set; }




    }
}
