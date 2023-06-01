using PedidosRestaurante.Models;
using PedidosRestaurante.Views.CategoriasViewTabbed;
using PedidosRestaurante.ViewsModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using checkBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambioMesaView : PopupPage
    {

        private readonly ObservableCollection<ClienteModel> PlatosCambio = new ObservableCollection<ClienteModel>();

        readonly FacturacionViewModel fvm = new FacturacionViewModel();
        private MesasModel item;
        private decimal valor;
        private ObservableCollection<ClienteModel> cliente;
        public CambioMesaView(ObservableCollection<ClienteModel> clientes, MesasModel item1, decimal v)
        {
            InitializeComponent();
            cliente = clientes;
            Titulo.Text = "DE MESA # " + cliente[0].Mesa.ToString() + " A MESA #" + item1.Mes_numero;
            foreach (var item in clientes)
            {
                item.Ischecked = false;
            }
            Platos.ItemsSource = clientes;
            item = item1;
            valor = v;
        }
        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var _ = ((checkBox)sender).BindingContext as ClienteModel;
            if (Btn_selectall.IsChecked)
            {
                Btn_selectall.IsChecked = false;
            }
            if (((checkBox)sender).IsChecked)
            {
                PlatosCambio.Add(_);
            }
            else
            {
                int position = 0;
                for (int i = 0; i < PlatosCambio.Count; i++)
                {
                    if (PlatosCambio[i].Lastid == _.Lastid)
                    {
                        position = i;
                        break;
                    }
                }
                PlatosCambio.RemoveAt(position);
            }
        }
        private void Btn_cancelar_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
        private async void Btn_cambiar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (PlatosCambio.Count > 0)
                {
                    fvm.CambiarArticulosMesa(PlatosCambio, item, valor, cliente);
                    await PopupNavigation.Instance.PopAsync();
                    Application.Current.MainPage = new NavigationPage(new Mesas());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }
        private void Btn_selectall_CheckChanged(object sender, EventArgs e)
        {
            var _ = ((checkBox)sender).BindingContext as ClienteModel;
            if (((checkBox)sender).IsChecked)
            {
                PlatosCambio.Clear();
                foreach (var item in cliente)
                {
                    item.Ischecked = true;
                    PlatosCambio.Add(item);
                }
                Platos.ItemsSource = null;
                Platos.ItemsSource = cliente;
            }
            else
            {
                PlatosCambio.Clear();
                foreach (var item in cliente)
                {
                    item.Ischecked = false;
                }
                Platos.ItemsSource = cliente;
            }
        }
    }
}