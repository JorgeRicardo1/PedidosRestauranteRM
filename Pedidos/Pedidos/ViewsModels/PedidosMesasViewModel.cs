using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PedidosRestaurante.ViewsModels
{
    public class PedidosMesasViewModel
    {
        readonly FacturacionViewModel fvm = new FacturacionViewModel();
        public ObservableCollection<PedidosMesasModel> PedidosMesas { get; set; }

        public string Nummesa { get; set; }
        public string Tipomesa { get; set; }
        public string Numfactura { get; set; }
        public string Valormesa { get; set; }
        public string Mesero { get; set; }
        public string Fechaingreso { get; set; }
        public PedidosMesasViewModel()
        {

        }

        public void ConsultaPedidosMesas(int mesa)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = culture;
            PedidosMesas = null;
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            List<PedidosMesasModel> result = new List<PedidosMesasModel>();
            var command = new MySqlCommand("SELECT * FROM xxxxmesa,xxxxmesm where xxxxmesa.id_mesa= xxxxmesm.id_mesa and xxxxmesa.id_mesa='" + mesa + "';", connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reader.GetString("mvm_mesa");
                    if (reader.GetString("mvm_mesa").Contains("B"))
                    {
                        Tipomesa = "Bar";
                    }
                    else
                    {
                        Tipomesa = "Mesa";
                    }
                    Nummesa = reader.GetString("mes_numero");
                    Valormesa = string.Format("{0:n0}", (int)reader.GetDecimal("mes_valor"));
                    Fechaingreso = reader.GetDateTime("mes_fecha").ToString();

                    result.Add(new PedidosMesasModel
                    {
                        Id_mesa = (int)reader.GetInt64("id_mesa"),
                        Mvm_cant = (int)reader.GetDecimal("mvm_cant"),
                        Mvm_nombre = reader.GetString("mvm_nombre"),
                        Mvm_valor = string.Format("{0:n0}", (int)reader.GetDecimal("mvm_valor")),
                        Mvm_codigo = reader.GetString("mvm_codigo"),
                        Mvm_neto = reader.GetDecimal("mvm_neto"),
                        Artivlr1_c = reader.GetDecimal("mvm_valor"),
                        Mvm_unidad = reader.GetString("mvm_unidad"),
                        Mvm_grupo = reader.GetString("mvm_grupo"),
                        Mvm_notas = reader.GetString("mvm_notas"),
                        Id_mesm = reader.GetInt32("id_mesm"),
                        Mes_estado = reader.GetInt32("mes_estado"),
                        Mes_tipo = reader.GetInt32("mes_tipo"),
                        Mvm_nit = reader.GetString("mvm_nit"),
                        Mvm_combi = reader.GetInt32("mvm_combi"),
                        Mvm_print = reader.GetInt32("mvm_print")
                    });
                }
                PedidosMesas = new ObservableCollection<PedidosMesasModel>(result);

            }
            else
            {
                result.Add(new PedidosMesasModel
                {
                    Mes_estado = 1
                });
                PedidosMesas = new ObservableCollection<PedidosMesasModel>(result);

            }
            reader.Close();
            connection.Close();
        }

        public ObservableCollection<MesasModel> MesasReferenciadas(string idmesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<MesasModel> mesas = new ObservableCollection<MesasModel>();
            var _ = new MySqlCommand("SELECT * FROM xxxxmesa where id_mesa='"+idmesa+"';", connection).ExecuteReader();
            if (_.HasRows)
            {
                while (_.Read())
                {
                    mesas.Add(new MesasModel
                    {
                        Id_mesa = _.GetInt32("id_mesa"),
                        Mes_numero = _.GetString("mes_numero")
                    });
                }
                _.Close();
            }
            connection.Close();
            return mesas;
        }
        public ObservableCollection<MesasModel> ConsultarMesasReferenciadas(int idmesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<MesasModel> mesas = new ObservableCollection<MesasModel>();
            var _ = new MySqlCommand("SELECT * FROM xxxxmesa where mes_refer='" + idmesa + "';", connection).ExecuteReader();
            if (_.HasRows)
            {
                while (_.Read())
                {
                    mesas.Add(new MesasModel
                    {
                        Id_mesa = _.GetInt32("id_mesa"),
                        Mes_numero = _.GetString("mes_numero")
                    });
                }
                
            }
            connection.Close();
            return mesas;
        }

        public void ConsultaMesasReferenciadas(int idmesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<MesasModel> mesas = new ObservableCollection<MesasModel>();
            var _ = new MySqlCommand("SELECT * FROM xxxxmesa where mes_refer='" + idmesa + "';", connection).ExecuteReader();
            if (_.HasRows)
            {
                while (_.Read())
                {
                    mesas.Add(new MesasModel
                    {
                        Id_mesa = _.GetInt32("id_mesa"),
                        Mes_numero = _.GetString("mes_numero")
                    });
                }
                _.Close();
                foreach (var item in mesas)
                {
                    new MySqlCommand("UPDATE xxxxmesa SET mes_refer= '' WHERE id_mesa ='" + item.Id_mesa + "';", connection).ExecuteNonQuery();
                    new MySqlCommand("UPDATE xxxxmesm SET mvm_refer='' WHERE id_mesa ='" + item.Id_mesa + "';", connection).ExecuteNonQuery();
                }
            }
            connection.Close();
        }
        public void Eliminarcomanda(int id_mesa, ObservableCollection<ClienteModel> clientes)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ConsultaMesasReferenciadas(id_mesa);
            new MySqlCommand("UPDATE xxxxmesa SET mes_estado = '1',mes_valor='0' WHERE id_mesa ='" + id_mesa + "';", connection).ExecuteNonQuery();            
            new MySqlCommand("DELETE FROM xxxxmesm WHERE id_mesa = '" + id_mesa + "';", connection).ExecuteNonQuery();
            new MySqlCommand("DELETE FROM xxxxcomp WHERE cmp_mesa = '" + id_mesa + "';", connection).ExecuteNonQuery();
            foreach (var item in clientes)
            {
                fvm.Actualizarstock(item.Codigo, (int)(fvm.Consultarcant(item.Codigo) - (int)(item.Cantidad)));
                foreach (var item1 in item.ArticulosXCliente)
                {
                    if (item1.Cmp_codigo == null)
                    {
                        var _1 = fvm.Consultarcant(item1.Cmp_item);
                        var t1 = _1 - 1;
                        fvm.Actualizarstock(item1.Cmp_item, (int)t1);
                    }
                    else
                    {
                        var _ = fvm.Consultarcant(item1.Cmp_codigo);
                        var t = _ - (int)item1.Cmp_cant;
                        fvm.Actualizarstock(item1.Cmp_codigo, (int)t);
                    }
                }
            }
            connection.Close();
        }
        public bool Getestadomesa(int mesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            var reader = new MySqlCommand("SELECT mes_estado FROM xxxxmesa where xxxxmesa.id_mesa='" + mesa + "';", connection).ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.GetInt32("mes_estado") == 2)
                {
                    connection.Close();
                    return true;
                }
                else
                {
                    connection.Close();
                    return false;
                }

            }
            connection.Close();
            return false;
        }
        public List<MesasModel> Mesasdisponibles(int idmesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            List<MesasModel> Mesasfree = new List<MesasModel>();
            var Mesas = new MySqlCommand("SELECT * FROM xxxxmesa where mes_refer='' and id_mesa != '" + idmesa + "';", connection);
            var reader = Mesas.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Mesasfree.Add(new MesasModel
                    {
                        Id_mesa = (int)reader.GetInt64("id_mesa"),
                        Mes_numero = reader.GetString("mes_numero")
                    });
                }
                reader.Close();
                connection.Close();
            }
            connection.Close();
            return Mesasfree;
        }
        public async Task<List<MesasModel>> Mesasdispo(int mesa, int idmesareferenciada)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            List<MesasModel> Mesasfree = new List<MesasModel>();
            var Mesas = new MySqlCommand("SELECT * FROM xxxxmesa where mes_refer='' and id_mesa !='"+idmesareferenciada+"' and id_mesa !='" + mesa + "';", connection);
            var reader = Mesas.ExecuteReader();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Mesasfree.Add(new MesasModel
                    {
                        Id_mesa = (int)reader.GetInt64("id_mesa"),
                        Mes_numero = reader.GetString("mes_numero")
                    });
                }
                reader.Close();
                connection.Close();
            }
            connection.Close();
            return Mesasfree;
        }
        public void Actualizarmesas(string mesa, Decimal valor, string mes_numero, int id_mesa, string fecha)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = culture;
            DateTime currentDateTime = Convert.ToDateTime(fecha);
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            //actulaizar mesa
            var mesanew = new MySqlCommand("UPDATE xxxxmesa SET  mes_estado = '2',mes_valor='" + valor + "',mes_fecha='" + currentDateTime + "' WHERE mes_numero ='" + mesa + "';", connection);
            //remover mesa anterior
            var mesasold = new MySqlCommand("UPDATE xxxxmesa SET mes_estado = '1',mes_valor='0' WHERE mes_numero ='" + mes_numero + "';", connection);
            //actulizar el pedido
            var articulos = new MySqlCommand("UPDATE xxxxmesm SET mvm_mesa = '" + mesa + "', id_mesa='" + id_mesa + "' WHERE mvm_mesa ='" + mes_numero + "';", connection);
            mesanew.ExecuteNonQuery();
            mesasold.ExecuteNonQuery();
            articulos.ExecuteNonQuery();
            connection.Close();
        }
    }

    //    string espacio = " ";
    //    String[] parametros = new String[array.Count + 1];
    //    parametros[0] = "SELECT * FROM xxxxmesa where mes_refer='" + espacio + "'";
    //            for (int i = 1; i<array.Count; i++)
    //            {
    //                if (string.IsNullOrEmpty(array[i].mes_refer))
    //                {
    //                    parametros[i] = " and id_mesa !='" + array[i].mes_refer + "'";
    //                }                
    //            }
    //            parametros[array.Count + 1] = ";";
    //string consulta = String.Join(String.Empty, parametros);



}
