using PedidosRestaurante.Models;
using PedidosRestaurante.ViewsModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticulosView : ContentPage
    {
        public delegate void SetArticulosEventHandler(object source, EventArgs args);

        public event SetArticulosEventHandler ArticuloCombinadoSet;
        public event SetArticulosEventHandler ArticuloSet;
        readonly FacturacionViewModel FacturacionViewModel = new FacturacionViewModel();
        readonly ObservableCollection<ArticulosModel> Artigrupo = new ObservableCollection<ArticulosModel>();
        readonly MesasViewModel MesasViewModel = new MesasViewModel();
        readonly int idmesa;
        readonly string  mesnumero;
        readonly int numcliente;
        private ArticulosModel oldDetail;
        private ArticulosModel detail;
        public ArticulosView(int id_mesa, string mes_numero, int v)
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await App.Context.Deletearti();
                await MesasViewModel.Consultaarticulos();
                //await FacturacionViewModel.ConsultaarticulosAsync();
                await FacturacionViewModel.ConsultaGruposArticulosAsync();
                foreach (var item in FacturacionViewModel.GrupoArticulos.OrderBy(w => w.Nombre))
                {
                    await FacturacionViewModel.ArticulosXGrupoArticulos(item.Artigrupo);
                    Artigrupo.Add(new ArticulosModel
                    {
                        NombreGrupo = item.Nombre,
                        Codigo = item.Artigrupo,
                        ArticulosXGrupo = FacturacionViewModel.Articulos                       
                    });
                }
            }).GetAwaiter().GetResult();
            idmesa = id_mesa;
            mesnumero = mes_numero;
            numcliente = v;
            ListaArticulos.ItemsSource = Artigrupo;
        }
        //Disparador de eventos       
        protected virtual void OnArticuloCombinadosSet()
        {
            ArticuloCombinadoSet?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnArticuloSet()
        {
            ArticuloSet?.Invoke(this, EventArgs.Empty);
        }
        private async void ListaArticulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var Articulo = (e.CurrentSelection.FirstOrDefault() as XxxxartiModel);

                if (Articulo.Librecnt == 0)
                {
                    var _ = FacturacionViewModel.Consultarstock(Articulo.Articodigo);
                    if (_ > 0)
                    {
                        if (Articulo.Articombi == 4)
                        {
                            ClientesView.Articulo = Articulo;
                            await Navigation.PopModalAsync();
                            OnArticuloCombinadosSet();
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            var result = await DisplayPromptAsync(Articulo.Artinomb, "Cantidad", keyboard: Keyboard.Numeric);
                            if (!string.IsNullOrEmpty(result))
                            {
                                if (_ >= decimal.Parse(result))
                                {
                                    Articulo.Articantidad = Int32.Parse(result);
                                    Articulo.Id_mesa = idmesa;
                                    Articulo.Artivalor = string.Format("{0:n0}", Articulo.Articantidad * Articulo.Artivlr1_c);
                                    var stock = FacturacionViewModel.Consultarstock(Articulo.Articodigo);
                                    if (stock > 0)
                                    {
                                        FacturacionViewModel.Agregararticulos(Articulo, Articulo.Artivlr1_c, mesnumero, numcliente, Articulo.Articombi);
                                        var u = FacturacionViewModel.Consultarcant(Articulo.Articodigo);
                                        FacturacionViewModel.Actualizarstock(Articulo.Articodigo, (int)(Articulo.Articantidad + u));
                                        //await Navigation.PopModalAsync();
                                        //OnArticulosSet();
                                    }
                                    else
                                    {
                                        await DisplayAlert("Información", "Articulo sin Stock!!!", "Aceptar");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("INFO", "La cantidad seleccionada es mayor al stock del articulo", "Aceptar");
                                }
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "El articulo no tiene existencias", "Aceptar");

                    }

                }
                else
                {
                    if (Articulo.Articombi == 4)
                    {
                        ClientesView.Articulo = Articulo;
                        await Navigation.PopModalAsync();
                        OnArticuloCombinadosSet();
                    }
                    else
                    {
                        var result = await DisplayPromptAsync(Articulo.Artinomb, "Cantidad", keyboard: Keyboard.Numeric);
                        if (!string.IsNullOrEmpty(result))
                        {
                            Articulo.Articantidad = Int32.Parse(result);
                            Articulo.Id_mesa = idmesa;
                            Articulo.Artivalor = string.Format("{0:n0}", Articulo.Articantidad * Articulo.Artivlr1_c);
                            var stock = FacturacionViewModel.Consultarstock(Articulo.Articodigo);
                            if (stock > -3000)//CAMBIOOO 04/05/2023
                            {
                                var id = FacturacionViewModel.Agregararticulos(Articulo, Articulo.Artivlr1_c, mesnumero, numcliente, Articulo.Articombi);
                                var u = FacturacionViewModel.Consultarcant(Articulo.Articodigo);
                                FacturacionViewModel.Actualizarstock(Articulo.Articodigo, (int)(Articulo.Articantidad + u));
                                //await Navigation.PopModalAsync();

                            }
                            else
                            {
                                await DisplayAlert("Información", "Articulo sin Stock!!!", "Aceptar");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Menu_dia_Clicked(object sender, EventArgs e)
        {
            ListaArticulos.ItemsSource = null;
            ListaArticulos.ItemsSource = PlatoViewModel.GetMenusDia();
        }
        private void ListaArticulos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            detail = e.Item as ArticulosModel;
            HideOrShowDetail(detail);
        }
        private void HideOrShowDetail(ArticulosModel detail)
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
        private void UpdateDetail(ArticulosModel detail)
        {
            int position = 0;
            for (int i = 0; i < Artigrupo.Count; i++)
            {
                if (Artigrupo[i].Codigo == detail.Codigo)
                {
                    position = i;
                    break;
                }
            }
            Artigrupo.RemoveAt(position);
            Artigrupo.Insert(position, detail);
        }
        protected override bool OnBackButtonPressed()
        {

            //PopupNavigation.Instance.PopAsync();
            OnArticuloSet();
            return base.OnBackButtonPressed();
        }
    }
}