using SQLite;

namespace PedidosRestaurante.Models
{
    public class ImpresoraModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Ip { get; set; }
    }
}
