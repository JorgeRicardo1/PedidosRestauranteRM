using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using PedidosRestaurante.Views.CategoriasViewTabbed;
using System;
using System.Collections.ObjectModel;

namespace PedidosRestaurante.ViewsModels
{
    public class PlatoViewModel
    {
        public static ObservableCollection<CombinadosModel> GetCombinados(string anacodigo)
        {

            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<CombinadosModel> _ = new ObservableCollection<CombinadosModel>();
            var command = new MySqlCommand("SELECT distinctrow(anaitem),anacodigo,xxxxarti.artiaplica,ananomb,usonombre,librecnt,artivlr1,articant,artiped_s " +
                "FROM xxxxanal,xxxxarti,xxxxusos,xxxxgrup,xxxxartv " +
                "where xxxxarti.articodigo = xxxxanal.anaitem and xxxxarti.artiaplica = xxxxusos.usocodigo and xxxxanal.anatiempo = '1'" +
                " and xxxxanal.anacodigo = '" + anacodigo + "' " +
                "and xxxxarti.artigrupo = xxxxgrup.codigo and xxxxartv.artVcodigo = xxxxanal.anaitem order by usonombre;", connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _.Add(new CombinadosModel
                    {
                        Anaitem = reader.GetString("anaitem"),
                        Anacodigo = reader.GetString("anacodigo"),
                        Artiaplica = reader.GetString("artiaplica"),
                        Ananomb = reader.GetString("ananomb"),
                        Usonombre = reader.GetString("usonombre"),
                        Librecnt = reader.GetDecimal("librecnt"),
                        Valor1 = reader.GetDecimal("artivlr1"),
                        Articant = reader.GetDecimal("articant"),
                        Artiped_s=reader.GetDecimal("artiped_s")
                    });
                }
                reader.Close();
                connection.Close();
            }
            return _;
        }
        public static void SetCombinadosPlatos(ObservableCollection<CombinadosModel> combinacionesSeleccionadas, int id_mesa, int lastid)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            foreach (var item in combinacionesSeleccionadas)
            {
                new MySqlCommand("INSERT INTO xxxxcomp " +
                    "(cmp_mesa,cmp_item,cmp_codigo,cmp_nomb,id_mesm,cmp_costo)" +
                "VALUES ('" + id_mesa + "','" + item.Anaitem + "','" + item.Anacodigo + "','" + item.Ananomb + "','" + lastid + "','" + (int)item.Valor1 + "');",
                connection).ExecuteNonQuery();
            }
            connection.Close();
        }
        public static ObservableCollection<XxxxartiModel> GetMenusDia()
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<XxxxartiModel> _ = new ObservableCollection<XxxxartiModel>();
            var command = new MySqlCommand("SELECT * FROM xxxxunit where xxxxunit.ind_factor = '1';", connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _.Add(new XxxxartiModel
                    {
                        Articodigo = reader.GetString("codigo"),
                        Artinomb = reader.GetString("nombre"),
                        Artivalor = string.Format("{0:n0}", (int)reader.GetDecimal("valor1_c")),
                        Artivlr1_c = reader.GetInt32("valor1_c"),
                        Articombi = reader.GetInt32("combinar"),
                        Artigrupo = reader.GetString("codigo")
                    });
                }
                reader.Close();
                connection.Close();
            }
            return _;
        }
        public static ObservableCollection<XxxxcompModel> GetArticulosXCliente(int mesa, string codigo, int comp_id_mesm)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            ObservableCollection<XxxxcompModel> _ = new ObservableCollection<XxxxcompModel>();
            var reader = new MySqlCommand("SELECT * FROM xxxxcomp where xxxxcomp.cmp_mesa = '" + mesa + "' " +
                "and xxxxcomp.cmp_codigo = '" + codigo + "' and xxxxcomp.id_mesm = '" + comp_id_mesm + "';", connection).ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _.Add(new XxxxcompModel
                    {
                        Id_comp = reader.GetInt32("id_comp"),
                        Cmp_mesa = reader.GetInt32("cmp_mesa"),
                        Cmp_item = reader.GetString("cmp_item"),
                        //Cmp_codigo = reader.GetString("cmp_codigo"),
                        Cmp_cant = reader.GetInt32("cmp_cant"),
                        Cmp_nomb = reader.GetString("cmp_nomb"),
                        Cmp_costo = reader.GetDecimal("cmp_costo"),
                        Id_mesm = reader.GetInt32("id_mesm"),
                    });
                }
                reader.Close();
                connection.Close();
            }
            return _;
        }
        public static void UpdatePlato(ObservableCollection<CombinadosModel> combinacionesSeleccionadas, int id_mesm, int id_mesa)
        {
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            new MySqlCommand("DELETE FROM xxxxcomp WHERE id_mesm ='" + id_mesm + "';", connection).ExecuteNonQuery();
            SetCombinadosPlatos(combinacionesSeleccionadas, id_mesa, id_mesm);
            connection.Close();
        }

        //------CAMBIO REALIZADO 10/05/2023 JORGE-------
        public static int GesDescuentosUsos(string uso)
        {
            decimal descuento = 0;
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            var reader = new MySqlCommand($"SELECT Usodescto FROM xxxxusos WHERE usonombre = '{uso}';", connection).ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    descuento = reader.GetDecimal("Usodescto");
                }
            }
            connection.Close();
            return Convert.ToInt32(descuento) ;
        }
        //----------------------------------------------
    }
}
