using MySqlConnector;
using PedidosRestaurante.Data;
using PedidosRestaurante.Models;
using PedidosRestaurante.Views;
using PedidosRestaurante.Views.CategoriasViewTabbed;
using PedidosRestaurante.ViewsModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace PedidosRestaurante
{
    public partial class App : Application
    {
        public static DataBase Context { get; set; }
        public static SQLiteAsyncConnection Connection { get; set; }
        public static MySqlConnection connectione;
        private List<EmpresasModel> empresa;
        private bool func = true;
        public App()
        {
            InitializeComponent();
            InitializeDatabase();
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
            //MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);     
        }
        private void InitializeDatabase()
        {
            var folderApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // se obtien la ruta de la carpeta local del aplicativo
            var dbPath = System.IO.Path.Combine(folderApp, "pedidos.db");
            Context = new DataBase(dbPath, Connection);
        }
        ///data/user/0/com.companyname.pedidos/files/.local/share/pedidos.db3
        protected override void OnStart()
        {
            var id = DependencyService.Get<PedidosRestaurante.IDevice>().DeviceID();
            Task.Run(async () =>
            {
                try
                {
                    var t8 = empresa = await App.Context.GetEmpresaAsync();
                    //verifica si hay conexion con el servidor
                    var connection = Conexion.ObtenerConexion();
                    connection.Open();
                    if (empresa.Count == 0)
                    {
                        func = false;
                        MainPage = new NavigationPage(new EmpresaView(null));
                        MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
                    }
                    else
                    {
                        if (func)
                        {

                            var empre = EmpresasViewModel.Validar(id, empresa[0].Empresa, connection);
                            connection.Close();
                            if (empre.Count > 0 && empre[0].Modulos.Equals("M12") && empre[0].Activar.Equals(empresa[0].Activar))
                            {
                                MainPage = new NavigationPage(new Mesas());
                                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
                            }
                            else
                            {
                                //--cambio jorge 30/05/2023---
                                empresa[0].Error = VerError(empre, empresa);
                                //---------------------------
                                MainPage = new NavigationPage(new EmpresaView(empresa));
                                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    if (empresa.Count == 0)
                    {
                        func = false;
                        MainPage = new NavigationPage(new EmpresaView(null));
                        MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
                    }
                    else
                    {
                        empresa[0].Error = ex.ToString();
                        MainPage = new NavigationPage(new EmpresaView(empresa));
                        MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
                    }
                }

            }).GetAwaiter().GetResult();
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private string VerError(ObservableCollection<EmpresasModel> empre, List<EmpresasModel> empresa)
        {
            string error = "";
            if (empre.Count <= 0)
            {
                error += "No se encuentra la mac en la base de datos \n";
                return error;
            }
            if (!empre[0].Modulos.Equals("M12"))
            {
                error += "No se puso el modulo \n";
            }
            if (!empre[0].Activar.Equals(empresa[0].Activar))
            {
                error += "Clave activar incorrecta \n";
            }

            return error;
        }
    }
}
