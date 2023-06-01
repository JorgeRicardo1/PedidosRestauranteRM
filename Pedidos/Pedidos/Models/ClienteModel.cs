using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PedidosRestaurante.Models
{
    public class ClienteModel
    {
        public int Id_mesm { get; set; }
        public int NumeroCliente { get; set; }
        public string Plato { get; set; }
        public List<XxxxartiModel> ArticulosCombinados { get; set; } = new List<XxxxartiModel>();
        public string Mesa { get; set; }
        public decimal Valor { get; set; }
        public bool IsVisible { get; set; }
        public int Tipomesa { get; set; }
        public string Nit { get; set; }
        public int Cantidad { get; set; }
        public int print { get; set; }
        public string Codigo { get; set; }

        public int Lastid { get; set; }
        public string notas { get; set; }
        public decimal Lbcanti { get; set; }

        public bool Ischecked { get; set; }
        public int Combi { get; set; }

        public ObservableCollection<XxxxcompModel> ArticulosXCliente { get; set; } = new ObservableCollection<XxxxcompModel>();
        public ObservableCollection<XxxxcompModel> ArticulosXcombi { get; set; } = new ObservableCollection<XxxxcompModel>();

    }
}
