using PedidosRestaurante.Models;
using PedidosRestaurante.ViewsModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Views.CategoriasViewTabbed
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mesas : ContentPage
    {
        readonly MesasViewModel MesasViewModel = new MesasViewModel();
        
        public Mesas()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //NavigationPage.SetHasNavigationBar(this, false);
        }

       
        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                BindingContext = null;
                MesasViewModel.ConsultaMesas();               
                BindingContext = MesasViewModel;
            }
            catch (Exception )
            {
                DisplayAlert("Informacion", "Sin conexion a Internet", "Aceptar");
            }
            refreshing.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {           
            try
            {
                BindingContext = null;
                MesasViewModel.ConsultaMesas();               
                BindingContext = MesasViewModel;
            }
            catch (Exception )
            {
                DisplayAlert("Informacion", "Sin conexion a Internet", "Aceptar");
            }
            refreshing.IsRefreshing = false;
        }

        private void Btn_selectmesa_Clicked(object sender, EventArgs e)
        {            
           var mesa = (sender as Button).CommandParameter as MesasModel;
            try
            {
                Navigation.PushAsync(new ClientesView(mesa), false);
                Application.Current.MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);
            }
            catch (Exception)
            {
                DisplayAlert("Informacion", "Sin conexion a Internet", "Aceptar");
            }
        }
    }
}