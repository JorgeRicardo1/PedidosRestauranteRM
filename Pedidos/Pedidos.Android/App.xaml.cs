using Pedidos.Data;
using Pedidos.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pedidos
{
    public partial class App : Application
    {
        public static DataBase Context { get; set; }
        public static Xxxx3rosData Context3ros { get; set; }
        public static XxxxartiData Contextarti { get; set; }

        public App()
        {

            InitializeComponent();
            InitializeDatabase();
            MainPage = new Mesas();
            MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
            
        }

        private void InitializeDatabase()
        {
            var folderApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // se obtien la ruta de la carpeta local del aplicativo
            var dbPath = System.IO.Path.Combine(folderApp, "pedidos.db");
            var t = Contextarti = new XxxxartiData(dbPath);

        }
        ///data/user/0/com.companyname.pedidos/files/.local/share/pedidos.db3
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
