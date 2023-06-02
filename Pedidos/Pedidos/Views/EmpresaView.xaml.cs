using PedidosRestaurante.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresaView : ContentPage
    {
        readonly private bool func = true;

        readonly private EmpresasModel empresamodel = new EmpresasModel();
        public EmpresaView(List<EmpresasModel> empresa)
        {
            InitializeComponent();
            Idinfo.Text = DependencyService.Get<PedidosRestaurante.IDevice>().DeviceID();
            if (empresa == null)
            {
                func = false;
                ServerPasword.Text = "*LiLo89*";
                Usuario.Text = "RmSoft20X";
            }
            else
            {
                ServerPasword.Text = "*LiLo89*";
                IpServer.Text = empresa[0].Ipserver;
                CodeEmpresa.Text = empresa[0].Empresa;
                Activar.Text = empresa[0].Activar;
                Usuario.Text = "RmSoft20X";
                MsgError.Text = empresa[0].Error;
            }
        }

        private async void Btnguardar_Clicked(object sender, EventArgs e)
        {
            empresamodel.Activar = Activar.Text;
            empresamodel.Empresa = CodeEmpresa.Text;
            empresamodel.Ipserver = IpServer.Text;
            empresamodel.ServerPasword = ServerPasword.Text;
            empresamodel.Usuario = Usuario.Text;
            if (!string.IsNullOrEmpty(Usuario.Text) && !string.IsNullOrEmpty(Activar.Text) && !string.IsNullOrEmpty(CodeEmpresa.Text) && !string.IsNullOrEmpty(IpServer.Text) && !string.IsNullOrEmpty(ServerPasword.Text))
            {
                if (func)
                {
                    empresamodel.Id = 1;
                    await App.Context.UpdateEmpresaAsync(empresamodel);
                    System.Environment.Exit(0);
                }
                else
                {
                    await App.Context.InsertEmpresaAsync(empresamodel);
                    System.Environment.Exit(0);
                }
            }
            else
            {
                await DisplayAlert("Informacion", "Algunos campos estan vacios", "Aceptar");
            }
        }

    }
}