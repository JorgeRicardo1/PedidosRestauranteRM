using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PedidosRestaurante.ViewsModels
{
    public class FacturacionViewModel
    {
        public ObservableCollection<XxxxartiModel> Articulos { get; set; }
        public ObservableCollection<XxxxartiModel> GrupoArticulos { get; set; }

        public FacturacionViewModel()
        {


        }
        //string nit, string nombrecliente,

        //public async Task ConsultaarticulosAsync()
        //{
        //    var toke = await App.Context.GetxxxxartiAsync();
        //    //Articulos = new ObservableCollection<XxxxartiModel>(toke);
        //}

        public async Task ConsultaGruposArticulosAsync()
        {
            GrupoArticulos = new ObservableCollection<XxxxartiModel>(await App.Context.GetGrupxxxxartiAsync());
        }

        public async Task ArticulosXGrupoArticulos(string codigo)
        {
            var toke = await App.Context.GetCodexxxxartiAsync(codigo);
            if (toke != null)
            {
                Articulos = new ObservableCollection<XxxxartiModel>(toke);
            }
        }
        public void Eliminararticulo(ClienteModel plato)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand("DELETE FROM xxxxmesm WHERE id_mesm = '" + plato.Id_mesm + "';", connection).ExecuteNonQuery();
            new MySqlCommand("DELETE FROM xxxxcomp WHERE id_mesm = '" + plato.Id_mesm + "';", connection).ExecuteNonQuery();
            Actualizarstock(plato.Codigo, (int)(Consultarcant(plato.Codigo) - (int)(plato.Cantidad)));
            foreach (var item in plato.ArticulosXCliente)
            {
                //var _ = Consultarcant(item.Cmp_codigo);
                //var t = _ - (int)item.Cmp_cant;
                //Actualizarstock(item.Cmp_codigo, (int)t);


                if (item.Cmp_codigo == null)
                {
                    var _1 = Consultarcant(item.Cmp_item);
                    var t1 = _1 - 1;
                    Actualizarstock(item.Cmp_item, (int)t1);
                }
                else
                {
                    var _ = Consultarcant(item.Cmp_codigo);
                    var t = _ - (int)item.Cmp_cant;
                    Actualizarstock(item.Cmp_codigo, (int)t);
                }
            }
            connection.Close();
        }
        //public void Actualizarcantidad(int id_mesm, int cant)
        //{
        //    var connection = Conexion.ObtenerConexion();
        //    connection.Open();
        //    var comman = new MySqlCommand("UPDATE xxxxmesm SET mvm_cant= '" + cant + "' WHERE id_mesm ='" + id_mesm + "';", connection);
        //    comman.ExecuteNonQuery();
        //    connection.Close();
        //}
        public void Actualizarprecio(int id_mesm, decimal valortotal)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand("UPDATE xxxxmesa SET mes_valor='" + (int)valortotal + "' WHERE id_mesa ='" + id_mesm + "';", connection).ExecuteNonQuery();
            connection.Close();
        }
        public void Actualizarestadomesa(int id_mesm)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand("UPDATE xxxxmesa SET mes_valor=0, mes_estado=1 WHERE id_mesa ='" + id_mesm + "';", connection).ExecuteNonQuery();
            connection.Close();
        }
        public int Agregararticulos(XxxxartiModel lista, decimal valortotal, string mes_numero, int nit, decimal combi)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            var command = new MySqlCommand("INSERT INTO xxxxmesm (id_mesa,mvm_mesa," +
                "mvm_codigo,mvm_cant,mvm_valor," +
                "mvm_nombre,mvm_unidad,mvm_grupo,mvm_notas,mvm_hora,mvm_neto,mvm_iva,mvm_nit,mvm_combi)" +
                "VALUES ('" + lista.Id_mesa + "'," +
                "'" + mes_numero + "'," +
                "'" + lista.Articodigo + "'," +
                "'" + lista.Articantidad + "'," +
                "'" + lista.Artivlr1_c + "'," +
                "'" + lista.Artinomb + "'," +
                "'" + lista.Mvm_unidad + "'," +
                "'" + lista.Mvm_grupo + "'," +
                "'" + lista.Mvm_notas + "'," +
                "'" + DateTime.Now.ToString("h:mm:ss") + "'," +
                "'" + lista.Mvm_neto + "'," +
                "'" + lista.Artiiva + "','" + nit + "','" + combi + "');SELECT LAST_INSERT_ID();", connection);
            int LastId = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            Actualizarmes(lista, valortotal);
            return LastId;
        }
        private void Actualizarmes(XxxxartiModel lista, decimal valortotal)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = culture;
            new MySqlCommand("UPDATE xxxxmesa " +
                "SET mes_estado = '2',mes_valor='" + (int)valortotal + "',mes_fecha='" + DateTime.Now + "' " +
                "WHERE id_mesa ='" + lista.Id_mesa + "';", connection).ExecuteNonQuery();
            connection.Close();
        }
        public void Actualizarnota(int id_mesm, string nota)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            var comman = new MySqlCommand("UPDATE xxxxmesm SET mvm_notas='" + nota + "' WHERE id_mesm ='" + id_mesm + "';", connection);
            comman.ExecuteNonQuery();
            connection.Close();
        }
        public void Actualizarstock(string codigo, int cants)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand("UPDATE xxxxartv SET artiped_s = '" + cants + "' where xxxxartv.artVcodigo = '" + codigo + "';", connection).ExecuteNonQuery();
            connection.Close();
        }
        public decimal Consultarstock(string codigo)
        {
            var connection = Conexion.ObtenerConexion();
            decimal stok = 0.0m;
            decimal cant = 0.0m;
            decimal cants = 0.0m;
            connection.Open();
            var _ = new MySqlCommand("select artiped_s,articant from xxxxartv where xxxxartv.artVcodigo = '" + codigo + "';", connection).ExecuteReader();
            if (_.HasRows)
            {
                while (_.Read())
                {
                    cant = _.GetDecimal("articant");
                    cants = _.GetDecimal("artiped_s");
                }
                stok = cant - cants;
            }
            connection.Close();
            return stok;
        }
        public decimal Consultarcant(string codigo)
        {
            var connection = Conexion.ObtenerConexion();
            decimal cants = 0.0m;
            connection.Open();
            var _ = new MySqlCommand("select artiped_s,articant from xxxxartv where xxxxartv.artVcodigo = '" + codigo + "';", connection).ExecuteReader();
            if (_.HasRows)
            {
                while (_.Read())
                {
                    cants = _.GetDecimal("artiped_s");
                }
            }
            connection.Close();
            return cants;
        }
        public void ActualizarImpresion(ObservableCollection<ClienteModel> clientes)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            foreach (var item in clientes)
            {
                new MySqlCommand("UPDATE xxxxmesm SET mvm_print = '1' where id_mesm = '" + item.Lastid + "';", connection).ExecuteNonQuery();
            }
            connection.Close();
        }
        public void CambiarArticulosMesa(ObservableCollection<ClienteModel> platosCambio, MesasModel mesa, decimal valor, ObservableCollection<ClienteModel> cliente)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = culture;
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            //actulizar el pedido
            var _ = 0.0m;
            foreach (var item in platosCambio)
            {
                var comman = new MySqlCommand("UPDATE xxxxmesm SET id_mesa= '" + mesa.Id_mesa + "',mvm_mesa='" + mesa.Mes_numero + "' WHERE id_mesm ='" + item.Lastid + "';", connection).ExecuteNonQuery();
                _ = item.Valor + _;
                if (item.Combi == 4)
                {
                    new MySqlCommand("UPDATE xxxxcomp SET cmp_mesa= '" + mesa.Id_mesa + "' WHERE id_mesm ='" + item.Lastid + "';", connection).ExecuteNonQuery();
                }
            }
            //nueva mesa
            new MySqlCommand("UPDATE xxxxmesa SET  mes_estado = '2',mes_valor='" + (int)_ + "',mes_fecha='" + DateTime.Now + "' " +
                "WHERE mes_numero ='" + mesa.Mes_numero + "';", connection).ExecuteNonQuery();
            //remover mesa anterior
            if (cliente.Count == platosCambio.Count)
            {
                new MySqlCommand("UPDATE xxxxmesa SET mes_estado = '1',mes_valor='0' WHERE mes_numero ='" + platosCambio[0].Mesa + "';", connection).ExecuteNonQuery();
            }
            connection.Close();
        }
        public void CombinarMesas(ObservableCollection<MesasModel> mesasCambio, int idmesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();         
            foreach (var item in mesasCambio)
            {
                new MySqlCommand("UPDATE xxxxmesa SET mes_refer= '" + idmesa + "' WHERE id_mesa ='" +  item.Id_mesa + "';", connection).ExecuteNonQuery();
                new MySqlCommand("UPDATE xxxxmesm SET mvm_refer= '" + idmesa + "' WHERE id_mesa ='" + item.Id_mesa + "';", connection).ExecuteNonQuery();
            }
            connection.Close();
        }

        //-----------CAMBIO REALIZADO 11/05/2023 JORGE----------
        public async Task AddIdPrint(string nombreMesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand($"INSERT INTO xxxxmesaz (`mes_numero`) VALUES('{nombreMesa}');", connection).ExecuteNonQuery();
            
            connection.Close();
        }

        public async Task<int> GetIdPrint()
        {
            try
            {
                int idPrint = 0;
                var connection = Conexion.ObtenerConexion();
                connection.Open();
                var reader = new MySqlCommand($"SELECT MAX(id_print) FROM xxxxmesaz;", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idPrint = reader.GetInt32("Max(id_print)");
                    }
                }

                connection.Close();
                return idPrint;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task ElminarLastIdPrint()
        {
            try
            {
                var connection = Conexion.ObtenerConexion();
                connection.Open();
                new MySqlCommand($"DELETE FROM xxxxmesaz WHERE id_print = (SELECT id FROM (SELECT MAX(id_print) as id FROM xxxxmesaz) as subquery);", connection).ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdatePrintMesa(int idMesa, int idMesaz)
        {
            try
            {
                var connection = Conexion.ObtenerConexion();
                connection.Open();
                new MySqlCommand($"UPDATE xxxxmesa SET id_mesaz = {idMesaz} WHERE id_mesa = '{idMesa}';", connection).ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<int> GetIdMesa(string NombreMesa)
        {
            try
            {
                int idMesa = 0;
                var connection = Conexion.ObtenerConexion();
                connection.Open();
                var reader = new MySqlCommand($"SELECT id_mesa FROM xxxxmesa WHERE mes_numero = '{NombreMesa}';", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idMesa = reader.GetInt32("id_mesa");
                    }
                }

                connection.Close();
                return idMesa;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        //-------------------------------------------------------
    }
}
