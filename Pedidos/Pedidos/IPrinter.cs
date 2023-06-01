using PedidosRestaurante.Models;
using System.Collections.ObjectModel;

namespace PedidosRestaurante
{
    public interface IPrinter
    {
        //void Print(string ipAddress, int port, IList<XxxxartiModel> linesToPrint);
        void Print(string ipAnddress, int portNmber, ObservableCollection<ClienteModel> clientes);
    }
}
