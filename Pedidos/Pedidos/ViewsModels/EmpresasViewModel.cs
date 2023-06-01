using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;

namespace PedidosRestaurante.ViewsModels
{
    public static class EmpresasViewModel
    {
        public static ObservableCollection<EmpresasModel> Validar(string mac, string empresa, MySqlConnection connection)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;

            //var connection = Conexion.ObtenerConexion();
            List<EmpresasModel> datos = new List<EmpresasModel>();
            //connection.Open();
            var reader = new MySqlCommand("SELECT * FROM empresas.llequipo where nro_mac='" + mac + "' and empresa='" + empresa + "';", connection).ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    datos.Add(new EmpresasModel
                    {
                        Empresa = reader.GetString("empresa"),
                        Nro_mac = reader.GetString("nro_mac"),
                        Activar = reader.GetString("activar"),
                        Modulos = reader.GetString("modulos"),
                    });
                }
            }
            reader.Close();
            new MySqlCommand("UPDATE empresas.llequipo " +
                "SET maquina ='" + DeviceInfo.Name + "',facceso='" + DateTime.Now + "',eq_notas='M12 02/05/2022'" +
                " WHERE nro_mac='" + mac + "';", connection).ExecuteNonQuery();
            //connection.Close();
            return new ObservableCollection<EmpresasModel>(datos);
        }


        public static string GetFecha()
        {
            using (var connection = Conexion.ObtenerConexion())
            {
                string horaactual = null;
                connection.Open();
                var fecha = new MySqlCommand("SELECT NOW() as fecha;", connection);
                var read = fecha.ExecuteReader();
                while (read.Read())
                {
                    horaactual = read[0].ToString();
                }
                connection.Close();
                return horaactual;
            }
        }
    }
}
