using PedidosRestaurante.Models;
using PedidosRestaurante.ViewsModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using checkBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CombinarMesasView : PopupPage
    {
        private readonly PedidosMesasViewModel pmvm = new PedidosMesasViewModel();
        private readonly ObservableCollection<MesasModel> MesasCambio = new ObservableCollection<MesasModel>();
        private ObservableCollection<MesasModel> MesasCombin = new ObservableCollection<MesasModel>();
        readonly FacturacionViewModel fvm = new FacturacionViewModel();
        private int idmesa;
        private int idmesareferenciada;
        public CombinarMesasView(MesasModel mesa, int idmesarefer)
        {
            InitializeComponent();
            idmesa = mesa.Id_mesa;
            idmesareferenciada = idmesarefer;
            NumeroMesa.Text = "MESA # " + mesa.Mes_numero;
            titulomesascombi.Text = "MESAS REFERENCIASDAS CON MESA # " + mesa.Mes_numero;            
            try
            {
                foreach (var item in pmvm.ConsultarMesasReferenciadas(mesa.Id_mesa))
                {
                    MesasCombin.Add(new MesasModel
                    {
                        Id_mesa = item.Id_mesa,
                        Mes_numero = item.Mes_numero,
                        Ischecked = true
                    });
                }
            }
            catch (Exception ex)
            {

                DisplayAlert("Error", "" + ex, "Aceptar");
            }
            Iniciar();
        }
        private async void Iniciar()
        {
            Mesas.ItemsSource = await pmvm.Mesasdispo(idmesa,idmesareferenciada);
            MesasCombinadas.ItemsSource = MesasCombin;
        }
        private void CheckBox_CheckChanged(object sender, System.EventArgs e)
        {
            var _ = ((checkBox)sender).BindingContext as MesasModel;
            if (((checkBox)sender).IsChecked)
            {
                MesasCambio.Add(_);
            }
            else
            {
                int position = 0;
                for (int i = 0; i < MesasCambio.Count; i++)
                {
                    if (MesasCambio[i].Id_mesa == _.Id_mesa)
                    {
                        position = i;
                        break;
                    }
                }
                MesasCambio.RemoveAt(position);
            }
        }
        private void Btn_cancelar_Clicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
        private async void Btn_combinar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (MesasCambio.Count > 0)
                {
                    fvm.CombinarMesas(MesasCambio, idmesa);
                }
                await PopupNavigation.Instance.PopAsync();
                //Application.Current.MainPage = new NavigationPage(new Mesas());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }
        private void checkbox_combinadas_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                var _ = ((checkBox)sender).BindingContext as MesasModel;
                if (!((checkBox)sender).IsChecked)
                {
                    int position = 0;
                    for (int i = 0; i < MesasCombin.Count; i++)
                    {
                        if (MesasCombin[i].Id_mesa == _.Id_mesa)
                        {
                            position = i;
                            break;
                        }
                    }
                    MesasCombin.RemoveAt(position);
                    pmvm.ConsultaMesasReferenciadas(idmesa);
                    fvm.CombinarMesas(MesasCombin, idmesa);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "" + ex, "Aceptar");
            }

        }
    }
}