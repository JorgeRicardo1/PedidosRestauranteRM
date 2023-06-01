using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PedidosRestaurante.ViewsModels
{
    public class MesasViewModel
    {
        public ObservableCollection<MesasModel> Mesas { get; set; }

        public int Mesasocupadas { get; set; }

        public int Mesaslibres { get; set; }
        //SELECT* FROM xxxxmesa where mes_refer=''; consulta para obtener mesas sin referencia 
        public MesasViewModel()
        {

        }
        public void ConsultaMesas()
        {
            Mesaslibres = 0;
            Mesasocupadas = 0;
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            List<MesasModel> mesas = new List<MesasModel>();
            var command = new MySqlCommand("SELECT * FROM xxxxmesa;", connection)
            {
                CommandTimeout = 60
            };
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                string image = string.Empty;
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader.GetString("mes_numero")))
                    {

                        if (reader.GetInt32("mes_tipo") == 1)
                        {
                            switch (reader.GetInt32("mes_estado"))
                            {
                                case 1:
                                    image = "tablevacia.png";
                                    Mesaslibres++;
                                    break;
                                case 2:
                                    image = "tablellena.png";
                                    Mesasocupadas++;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            if (reader.GetInt32("mes_tipo") == 2)
                            {
                                switch (reader.GetInt32("mes_estado"))
                                {
                                    case 1:
                                        image = "barrelvacio.png";
                                        Mesaslibres++;
                                        break;
                                    case 2:
                                        image = "barrel.png";
                                        Mesasocupadas++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                if (reader.GetInt32("mes_tipo") == 3)
                                {
                                    switch (reader.GetInt32("mes_estado"))
                                    {
                                        case 1:
                                            image = "scooter.png";
                                            Mesaslibres++;
                                            break;
                                        case 2:
                                            image = "delivery.png";
                                            Mesasocupadas++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        mesas.Add(new MesasModel
                        {
                            Id_mesa = reader.GetInt32("id_mesa"),
                            Image = image,
                            Mes_numero = reader.GetString("mes_numero"),
                            Mes_estado = reader.GetInt32("mes_estado"),
                            Mes_tipo = reader.GetInt32("mes_tipo"),
                            Mes_refer = reader.GetString("mes_refer")
                        });
                    }
                }//CAMBIO REALIZADO EL 08/05/2023(Ordenar mesa alfabeticamente)
                var mesasOrdenadas = mesas.OrderBy(x => x.Mes_numero);
                Mesas = new ObservableCollection<MesasModel>(mesasOrdenadas);

            }
            connection.Close();
        }

        //consulta para traer los articulos  and articant-artiped_s>0

        public async Task Consultaarticulos()
        {
            var _ = PlatoViewModel.GetMenusDia();
            var connection = Conexion.ObtenerConexion();
            connection.Open();
            List<XxxxartiModel> arti = new List<XxxxartiModel>();
            var command = new MySqlCommand("select articodigo,articant,artinomb, artiunidad, artigrupo,artivlr1_c,artiiva,articombi,artiped_s,nombre,librecnt " +
                " from xxxxarti,xxxxartv,xxxxgrup where  xxxxarti.artigrupo = xxxxgrup.codigo and  xxxxarti.articodigo = xxxxartv.artvcodigo;", connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader.GetDecimal("articombi") == 4)
                        {
                            foreach (var item in _)
                            {
                                var t = reader.GetString("articodigo");
                                if (item.Articodigo.Contains(t))
                                {
                                    arti.Add(new XxxxartiModel
                                    {
                                        Articodigo = reader.GetString("articodigo"),
                                        Artinomb = reader.GetString("artinomb"),
                                        Artivalor = string.Format("{0:n0}", (int)reader.GetDecimal("artivlr1_c")),
                                        Artiunidad = reader.GetString("artiunidad"),
                                        Artigrupo = reader.GetString("artigrupo"),
                                        Articodi2 = "",
                                        Articantidad = 0,
                                        //Articant = reader.GetDecimal("articant"),
                                        Id_mesa = 0,
                                        Mvm_mesa = "",
                                        Mvm_codigo = "",
                                        Mvm_valor = "",
                                        Mvm_unidad = "",
                                        Mvm_notas = "",
                                        Mvm_grupo = "",
                                        Artivlr1_c = reader.GetDecimal("artivlr1_c"),
                                        Artiiva = reader.GetInt32("artiiva"),
                                        Articombi = reader.GetDecimal("articombi"),
                                        Artiped_s = reader.GetDecimal("artiped_s"),
                                        Librecnt = reader.GetDecimal("librecnt"),
                                        Nombre = reader.GetString("nombre")
                                    });
                                    break;
                                }
                            }
                        }
                        else
                        {
                            arti.Add(new XxxxartiModel
                            {
                                Articodigo = reader.GetString("articodigo"),
                                Artinomb = reader.GetString("artinomb"),
                                Artivalor = string.Format("{0:n0}", (int)reader.GetDecimal("artivlr1_c")),
                                Artiunidad = reader.GetString("artiunidad"),
                                Artigrupo = reader.GetString("artigrupo"),
                                Articodi2 = "",
                                Articantidad = 0,
                                Articant = reader.GetDecimal("articant"),
                                Id_mesa = 0,
                                Mvm_mesa = "",
                                Mvm_codigo = "",
                                Mvm_valor = "",
                                Mvm_unidad = "",
                                Mvm_notas = "",
                                Mvm_grupo = "",
                                Artivlr1_c = reader.GetDecimal("artivlr1_c"),
                                Artiiva = reader.GetInt32("artiiva"),
                                Articombi = reader.GetDecimal("articombi"),
                                Artiped_s = reader.GetDecimal("artiped_s"),
                                Librecnt = reader.GetDecimal("librecnt"),
                                Nombre = reader.GetString("nombre")
                            });
                        }
                    }
                    await App.Context.InsertxxxxartiAsync(arti);
                }
                catch (Exception)
                {
                }

            }
            connection.Close();


        }
    }
}

