using System.Collections.ObjectModel;

namespace PedidosRestaurante.Models
{
    public class ArticulosModel
    {
        public string NombreGrupo { get; set; }
        public string Codigo { get; set; }
        public bool IsVisible { get; set; }
        public ObservableCollection<XxxxartiModel> ArticulosXGrupo { get; set; } = new ObservableCollection<XxxxartiModel>();


    }
}
