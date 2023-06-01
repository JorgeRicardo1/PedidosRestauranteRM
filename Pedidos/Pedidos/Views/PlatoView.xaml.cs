using PedidosRestaurante.Models;
using PedidosRestaurante.ViewsModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using checkBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlatoView : ContentPage
    {
        public delegate void SetArticulosEventHandler(object source, EventArgs args);
        public event SetArticulosEventHandler CombinadoSet;

        private CombinadosModel oldDetail;
        private CombinadosModel detail;
        private readonly ObservableCollection<CombinadosModel> combinado = new ObservableCollection<CombinadosModel>();
        private readonly ObservableCollection<CombinadosModel> combinado2 = new ObservableCollection<CombinadosModel>();

        private readonly ObservableCollection<CombinadosModel> articulosold = new ObservableCollection<CombinadosModel>();
        private readonly ObservableCollection<CombinadosModel> CombinacionesSeleccionadas = new ObservableCollection<CombinadosModel>();
        readonly bool oldarti = true;
        private readonly FacturacionViewModel fvm = new FacturacionViewModel();
        private readonly XxxxartiModel Articulo;
        readonly int id_mesa;
        readonly string mes_numero;
        readonly int cout;
        public PlatoView(XxxxartiModel arti, int idmesa, string mesnumero, int count)
        {
            InitializeComponent();
            id_mesa = idmesa;
            mes_numero = mesnumero;
            cout = count;
            Articulo = arti;
            Articulo.Articantidad = 1;
            Articulo.Id_mesa = idmesa;
            titulo.Text = arti.Artinomb;
            var _ = PlatoViewModel.GetCombinados(arti.Articodigo);
            foreach (var item in _)
            {
                foreach (var old in articulosold)
                {
                    if (item.Artiaplica.Equals(old.Artiaplica))
                    {
                        oldarti = false;
                        break;
                    }
                    oldarti = true;
                }
                if (oldarti)
                {
                    articulosold.Add(new CombinadosModel { Artiaplica = item.Artiaplica });
                    ObservableCollection<CombinadosModel> articom = new ObservableCollection<CombinadosModel>();
                    foreach (var item2 in _)
                    {
                        if (item2.Artiaplica.Equals(item.Artiaplica))
                        {
                            articom.Add(new CombinadosModel
                            {
                                Anaitem = item2.Anaitem,
                                Anacodigo = item2.Anacodigo,
                                Artiaplica = item2.Artiaplica,
                                Ananomb = item2.Ananomb,
                                Usonombre = item2.Usonombre,
                                Librecnt = item2.Librecnt,
                                Valor1 = item2.Valor1,
                                Articant = item2.Articant-item2.Artiped_s
                            });
                        }
                    }
                    combinado.Add(new CombinadosModel
                    {
                        Articulonombre = articom[0].Usonombre,
                        Articuloscombinados = articom,
                        IsVisible = false
                    });
                }
            }
            Listascombinados.ItemsSource = combinado;
        }
        protected virtual void OnCombinadoSet()
        {
            CombinadoSet?.Invoke(this, EventArgs.Empty);
        }
        private void Listascombinados_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            detail = e.Item as CombinadosModel;
            foreach (var item in detail.Articuloscombinados)
            {
                foreach (var comb in CombinacionesSeleccionadas)
                {
                    if (item.Anaitem == comb.Anaitem)
                    {
                        item.Ischecked = true;
                        break;
                    }
                }
            }
            HideOrShowDetail(detail);
        }
        private void HideOrShowDetail(CombinadosModel detail)
        {
            if (oldDetail == detail)
            {
                detail.IsVisible = !detail.IsVisible;
                UpdateDetail(detail);
            }
            else
            {
                if (oldDetail != null)
                {
                    oldDetail.IsVisible = false;
                    UpdateDetail(oldDetail);
                }
                detail.IsVisible = true;
                UpdateDetail(detail);
            }
            oldDetail = detail;
        }
        private void UpdateDetail(CombinadosModel detail)
        {
            var index = combinado.IndexOf(detail);
            combinado.Remove(detail);
            combinado.Insert(index, detail);
        }
        private void CheckBox_CheckChanged(object sender, System.EventArgs e)
        {
            var _ = ((checkBox)sender).BindingContext as CombinadosModel;
            if (((checkBox)sender).IsChecked)
            {
                CombinacionesSeleccionadas.Add(_);
            }
            else
            {
                CombinacionesSeleccionadas.RemoveAt(CombinacionesSeleccionadas.IndexOf(_));
            }
        }
        private async void BtnGuardar_Clicked(object sender, System.EventArgs e)
        {
            BtnGuardar.IsEnabled = false;
            bool estock = true;
            string nombre = string.Empty;
            if (CombinacionesSeleccionadas.Count > 0)
            {
                foreach (var item in CombinacionesSeleccionadas)
                {
                    if (item.Librecnt == 0)
                    {
                        var stock = fvm.Consultarstock(item.Anaitem);
                        if (stock <= 0)
                        {
                            estock = false;
                            nombre = item.Ananomb;
                            break;
                        }
                        else
                        {
                            var _ = fvm.Consultarcant(item.Anaitem);
                            fvm.Actualizarstock(item.Anaitem, (int)(1 + _));
                        }
                    }
                }
                if (estock)
                {
                    //-----CAMBIO REALIZADO 10/05/2023 JORGE-----
                    decimal valorDescuento;
                    if (combinado.Count != CombinacionesSeleccionadas.Count)
                    {
                        valorDescuento = calcularDescuento();
                        Articulo.Artivlr1_c -= valorDescuento;
                        
                    }
                    //-------------------------------------
                    var _ = fvm.Agregararticulos(Articulo, Articulo.Artivlr1_c , mes_numero, cout, Articulo.Articombi);
                    PlatoViewModel.SetCombinadosPlatos(CombinacionesSeleccionadas, id_mesa, _);
                    Articulo.comp_id_mesm = _;
                    ClientesView.Articulo = Articulo;
                    OnCombinadoSet();
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Información", nombre + "sin Stock!!!", "Aceptar");
                    BtnGuardar.IsEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("Información", "No sean seleccionado elementos!!!", "Aceptar");
                BtnGuardar.IsEnabled = true;
            }
        }

        //-----CAMBIO REALIZADO 10/05/2023--------------
        private decimal calcularDescuento()
        {
            bool descuento = true;
            decimal valorDescuento = 0;
            foreach (var item in combinado)
            {
                descuento = true;
                foreach (var item2 in CombinacionesSeleccionadas)
                {
                    if (item.Articulonombre == item2.Usonombre)
                    {
                        descuento = false;
                        break;
                    }
                }
                if (descuento)
                {
                    valorDescuento = PlatoViewModel.GesDescuentosUsos(item.Articulonombre);
                }
            }
            return valorDescuento;
           
        }
        //----------------------------------------
    }
}