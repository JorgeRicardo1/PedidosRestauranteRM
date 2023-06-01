using PedidosRestaurante.Models;
using PedidosRestaurante.Views.CategoriasViewTabbed;
using PedidosRestaurante.ViewsModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using checkBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarPlatoView : ContentPage
    {
        private CombinadosModel oldDetail;
        private CombinadosModel detail;
        readonly int id_mesm;
        readonly int id_mesa;
        readonly bool oldarti = true;
        private readonly ObservableCollection<CombinadosModel> combinado = new ObservableCollection<CombinadosModel>();
        private readonly ObservableCollection<CombinadosModel> CombinacionesSeleccionadas = new ObservableCollection<CombinadosModel>();
        private readonly ObservableCollection<CombinadosModel> articulosold = new ObservableCollection<CombinadosModel>();
        public EditarPlatoView(ClienteModel plato, int idmesa)
        {
            InitializeComponent();
            id_mesm = plato.Lastid;
            id_mesa = idmesa;
            var _ = PlatoViewModel.GetCombinados(plato.Codigo);
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
                            foreach (var pl in plato.ArticulosXCliente)
                            {
                                if (item2.Anaitem == pl.Cmp_item)
                                {
                                    CombinacionesSeleccionadas.Add(new CombinadosModel
                                    {
                                        Anaitem = pl.Cmp_item,
                                        Anacodigo = plato.Codigo,
                                        Ananomb = pl.Cmp_nomb,
                                        Valor1 = pl.Cmp_costo
                                    });
                                    break;
                                }
                            }
                            articom.Add(new CombinadosModel
                            {
                                Anaitem = item2.Anaitem,
                                Anacodigo = item2.Anacodigo,
                                Artiaplica = item2.Artiaplica,
                                Ananomb = item2.Ananomb,
                                Usonombre = item2.Usonombre,
                                Valor1 = item2.Valor1
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

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var _ = ((checkBox)sender).BindingContext as CombinadosModel;
            if (((checkBox)sender).IsChecked)
            {
                CombinacionesSeleccionadas.Add(_);
            }
            else
            {
                int position = 0;
                for (int i = 0; i < CombinacionesSeleccionadas.Count; i++)
                {
                    if (CombinacionesSeleccionadas[i].Anaitem == _.Anaitem)
                    {
                        position = i;
                        break;
                    }
                }
                CombinacionesSeleccionadas.RemoveAt(position);
            }
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                BtnGuardar.IsEnabled = false;
                if (CombinacionesSeleccionadas.Count > 0)
                {
                    PlatoViewModel.UpdatePlato(CombinacionesSeleccionadas, id_mesm, id_mesa);
                    Application.Current.MainPage = new NavigationPage(new Mesas());
                }
                else
                {
                    await DisplayAlert("Información", "No sean seleccionado elementos!!!", "Aceptar");
                    BtnGuardar.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Información", "" + ex, "Aceptar");
            }

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

        //-----CAMBIO REALIZADO 29/05/2023--------------
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