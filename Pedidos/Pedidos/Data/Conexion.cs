using MySqlConnector;
using PedidosRestaurante.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PedidosRestaurante.Data
{
    public class Conexion
    {
        public static MySqlConnection ObtenerConexion()
        {
            var empresa = new List<EmpresasModel>();
            Task.Run(async () =>
            {
                empresa = await App.Context.GetEmpresaAsync();
            }).GetAwaiter().GetResult();
            if (empresa.Count > 0)
            {
                //MySqlConnection connection = new MySqlConnection("Server='" + empresa[0].Ipserver + "';User ID=RmSoft20X;Password='" + empresa[0].ServerPasword + "';Database='" + empresa[0].Empresa + "'");
                MySqlConnection connection = new MySqlConnection("Server='" + empresa[0].Ipserver + "';User ID='" + empresa[0].Usuario + "';Password='" + empresa[0].ServerPasword + "';Database='" + empresa[0].Empresa.ToLower() + "'");
                //MySqlConnection connection = new MySqlConnection("Server=186.147.92.158;User ID=tr;Password=1011;Database='a000'");
                //var connection = new MySqlConnection("Server=192.168.1.128;User ID=Facturar;Password=Facturar123;Database='a000'");
                //MySqlConnection connection = new MySqlConnection("Server=192.168.1.243;User ID=root;Password=1011;Database=a000");
                return connection;
            }
            return null;
        }

    }
}
