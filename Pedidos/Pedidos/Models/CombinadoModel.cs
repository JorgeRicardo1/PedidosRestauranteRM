using System.Collections.ObjectModel;

namespace PedidosRestaurante.Models
{
    public class CombinadosModel
    {
        public string Anaitem { get; set; }
        public string Anacodigo { get; set; }
        public string Artiaplica { get; set; }
        public string Ananomb { get; set; }
        public string Usonombre { get; set; }
        public bool IsVisible { get; set; }
        public string Articulonombre { get; set; }
        public decimal Librecnt { get; set; }
        public decimal Valor1 { get; set; }
        public bool Ischecked { get; set; }
        public decimal Articant { get; set; }
        public decimal Artiped_s { get; set; }
        public ObservableCollection<CombinadosModel> Articuloscombinados { get; set; } = new ObservableCollection<CombinadosModel>();



    }
}
