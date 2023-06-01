using PedidosRestaurante.Models;
using PedidosRestaurante.Views.CategoriasViewTabbed;
using PedidosRestaurante.ViewsModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientesView : ContentPage
    {
        readonly PedidosMesasViewModel PedidosMesasViewMode = new PedidosMesasViewModel();
        private static XxxxartiModel _articulo;
        private readonly XxxxartiModel Articulos = new XxxxartiModel();
        readonly ObservableCollection<ClienteModel> Clientes = new ObservableCollection<ClienteModel>();
        private readonly FacturacionViewModel fvm = new FacturacionViewModel();

       
        public static XxxxartiModel Articulo
        {
            get { return _articulo; }
            set { _articulo = value; }
        }
        readonly int idmesa;
        readonly string mesnumero;
        readonly int idmesareferenciada;
        readonly MesasModel mesa;
        private ClienteModel oldDetail;
        private ClienteModel detail;
        private PedidosMesasViewModel pmvm;

        private int Clienteindex;
        private bool AdicionalXcliente = true;
        private bool ActivacionArticulos = true;
        public ClientesView(MesasModel mes)
        {
            InitializeComponent();
            mesa = mes;
            //PedidosMesasViewMode.ConsultaPedidosMesas(id_mesa);
            idmesa = mesa.Id_mesa;
            mesnumero = mesa.Mes_numero;
            framereferencia.IsVisible = false;
            try
            {
                foreach (var item in PedidosMesasViewMode.MesasReferenciadas(mesa.Mes_refer))
                {
                    framereferencia.IsVisible = true;
                    textoreferencia.Text = "mesa referenciada con mesa " + item.Mes_numero;
                    idmesareferenciada = item.Id_mesa;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Informacion", ex.Message, "Aceptar");
            }

            Title = "MESA #" + mesnumero;
            ActualizarVista();
        }
        private XxxxartiModel ArticulosReturn(PedidosMesasModel item)
        {
            Articulos.Artinomb = item.Mvm_nombre;
            Articulos.Artivlr1_c = item.Artivlr1_c;
            Articulos.Articodigo = item.Mvm_codigo;
            Articulos.comp_id_mesm = item.Id_mesm;
            Articulos.Tipo_mesa = item.Mes_tipo;
            Articulos.Nit = item.Mvm_nit;
            Articulos.Articantidad = (int)item.Mvm_cant;
            Articulos.Id_mesm = item.Id_mesm;
            Articulos.Articombi = item.Mvm_combi;
            Articulos.Mvm_printing = item.Mvm_print;
            Articulos.Mvm_notas = item.Mvm_notas;
            Articulos.combinado = item.Mvm_combi;
            //Articulos.Librecnt 
            return Articulos;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AgregarPlato.IsEnabled = true;
        }
        //disparador de eventos para agregar articulos
        public async void OnArtiCombinadoSet(object source, EventArgs e)
        {
            var platoview = new PlatoView(Articulo, idmesa, mesnumero, Clientes.Count + 1);
            platoview.CombinadoSet += this.OnCombinadoSet;
            await Navigation.PushModalAsync(platoview);
        }
        //disparador de eventos para agregar articulos
        public void OnCombinadoSet(object source, EventArgs e)
        {
            Articulo.Nit = (Clientes.Count + 1).ToString();
            //var _ = fvm.Consultarcant(Articulo.Articodigo);
            //fvm.Actualizarstock(Articulo.Articodigo, (int)(Articulo.Articantidad + _));
            AgregarArticulo(Articulo);
            ActualizarVista();
        }
        public void OnArtiSet(object source, EventArgs e)
        {
            ActualizarVista();
        }
        private void AgregarPlato_Clicked(object sender, EventArgs e)
        {
            AgregarPlato.IsEnabled = false;
            Clienteindex = Clientes.Count + 1;
            VistaArticulo();
        }
        private void Listaclientes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            detail = e.Item as ClienteModel;
            if (detail.ArticulosXCliente.Count > 0)
            {
                HideOrShowDetail(detail);
            }

        }
        private void HideOrShowDetail(ClienteModel detail)
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
        private void UpdateDetail(ClienteModel detail)
        {
            int position = 0;
            for (int i = 0; i < Clientes.Count; i++)
            {
                if (Clientes[i].Lastid == detail.Lastid)
                {
                    position = i;
                    break;
                }
            }
            Clientes.RemoveAt(position);
            Clientes.Insert(position, detail);
        }
        //redireciona a la vista de articulos
        private async void VistaArticulo()
        {
            var articulosview = new ArticulosView(idmesa, mesnumero, Clienteindex);
            articulosview.ArticuloCombinadoSet += this.OnArtiCombinadoSet;
            articulosview.ArticuloSet += this.OnArtiSet;
            await Navigation.PushModalAsync(articulosview);
        }
        private async void AgregararticuloInde(XxxxartiModel articulo)
        {
            try
            {
                Clientes.Add(new ClienteModel
                {
                    Plato = articulo.Artinomb,
                    Valor = articulo.Artivlr1_c * articulo.Articantidad,
                    Tipomesa = articulo.Tipo_mesa,
                    Nit = articulo.Nit,
                    Cantidad = articulo.Articantidad,
                    Mesa = mesnumero,
                    Id_mesm = articulo.Id_mesm,
                    Codigo = articulo.Articodigo,
                    print = articulo.Mvm_printing,
                    notas = articulo.Mvm_notas,
                    Lastid = articulo.comp_id_mesm,
                    //ArticulosXCliente = PlatoViewModel.GetArticulosXCliente(idmesa, articulo.Articodigo, articulo.comp_id_mesm)
                });
                Recalcularvalor();
                Listaclientes.ItemsSource = null;
                Listaclientes.ItemsSource = Clientes;

            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "Salir");
            }

        }
        private async void AgregarArticulo(XxxxartiModel articulo)
        {
            try
            {
                Clientes.Add(new ClienteModel
                {
                    Plato = articulo.Artinomb,
                    Valor = articulo.Artivlr1_c,
                    Tipomesa = articulo.Tipo_mesa,
                    Nit = articulo.Nit,
                    Cantidad = articulo.Articantidad,
                    Mesa = mesnumero,
                    Id_mesm = articulo.Id_mesm,
                    Codigo = articulo.Articodigo,
                    print = articulo.Mvm_printing,
                    Lastid = articulo.comp_id_mesm,
                    Combi = articulo.combinado,
                    notas = articulo.Mvm_notas,
                    ArticulosXCliente = PlatoViewModel.GetArticulosXCliente(idmesa, articulo.Articodigo, articulo.comp_id_mesm)
                });
                if (pmvm != null)
                {
                    var _ = Clientes.Count;
                    foreach (var item in pmvm.PedidosMesas)
                    {
                        if (articulo.Nit == item.Mvm_nit && item.Mvm_combi != 4)
                        {
                            Clientes[_ - 1].ArticulosXCliente.Add(new XxxxcompModel
                            {
                                Cmp_nomb = item.Mvm_nombre,
                                Cmp_cant = item.Mvm_cant,
                                Cmp_codigo = item.Mvm_codigo,
                            });
                            Clientes[_ - 1].Valor = Clientes[_ - 1].Valor + (item.Artivlr1_c * item.Mvm_cant);
                        }
                    }
                }
                Recalcularvalor();
                Listaclientes.ItemsSource = null;
                Listaclientes.ItemsSource = Clientes;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "" + ex, "Salir");
            }
        }
       
        private async void EliminarComanda_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool result = await DisplayAlert("Eliminar Comanda", "¿Desea eliminar la comanda?", "Si", "No");
                if (result)
                {
                    if (PedidosMesasViewMode.Getestadomesa(idmesa))
                    {
                        PedidosMesasViewMode.Eliminarcomanda(idmesa, Clientes);
                        Application.Current.MainPage = new NavigationPage(new Mesas());
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new Mesas());
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "" + ex, "Salir");
            }

        }
        private async void ButtoneliminarPlato_Clicked(object sender, EventArgs e)
        {
            try
            {
                ClienteModel _ = (ClienteModel)(sender as MenuItem).CommandParameter;
                var indice = Clientes.IndexOf(_);
                fvm.Eliminararticulo(_);
                Clientes.RemoveAt(indice);
                if (Clientes.Count == 0)
                {
                    fvm.Actualizarestadomesa(idmesa);
                }
                Listaclientes.ItemsSource = null;
                Listaclientes.ItemsSource = Clientes;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }
        private async void ImprimirComanda_Clicked(object sender, EventArgs e)
        {
            var printer = DependencyService.Get<PedidosRestaurante.IPrinter>();
            var ip = await App.Context.GetprinterAsync();
            if (printer == null)
            {
                await DisplayAlert("ERROR", "No implemnetacion", "Aceptar");
                return;
            }
            try
            {
                string ipAnddress = ip[0].Ip;
                int portNmber = 9100;
                ObservableCollection<ClienteModel> PrintClientes = new ObservableCollection<ClienteModel>();
                foreach (var item in Clientes)
                {
                    if (item.print == 0)
                    {
                        PrintClientes.Add(item);
                        //item.print = 1;
                    }
                }
                if (PrintClientes.Count > 0)
                {
                    
                    printer.Print(ipAnddress, portNmber, PrintClientes);
                    await DisplayAlert("Informativo", "Esta imprimiendo", "Aceptar");
                    
                    foreach (var item in Clientes)
                    {
                        item.print = 1;
                    }
                    fvm.ActualizarImpresion(PrintClientes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var result = await DisplayPromptAsync("ERROR", "Digite la IP de la impresora");
                if (ip.Count == 0)
                {

                    if (!string.IsNullOrEmpty(result))
                    {
                        ImpresoraModel impresora = new ImpresoraModel
                        {
                            Ip = result
                        };
                        await App.Context.InsertprinterAsync(impresora);
                    }
                }
                else
                {
                    ImpresoraModel impresora = new ImpresoraModel
                    {
                        Id = 1,
                        Ip = result
                    };
                    await App.Context.UpdateprinterAsync(impresora);
                }

            }

        }
        private void ButtonAgregaradicional_Clicked(object sender, EventArgs e)
        {
            try
            {
                var _ = (ClienteModel)(sender as MenuItem).CommandParameter;
                if (_.Combi == 4)
                {
                    Clienteindex = int.Parse(_.Nit);
                    AdicionalXcliente = true;
                    VistaArticulo();
                }
                else
                {
                    DisplayAlert("Informacion", "El articulo no es combinado", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }
        private decimal Recalcularvalor()
        {
            var _ = 0.0m;
            foreach (var item in Clientes)
            {
                _ = item.Valor + _;
            }
            Title = "MESA #" + mesnumero + " -$ " + string.Format("{0:n0}", _);
            fvm.Actualizarprecio(idmesa, _);
            return _;
        }
        private async void ButtonEditar_Clicked(object sender, EventArgs e)
        {
            var _ = (ClienteModel)(sender as MenuItem).CommandParameter;
            if (_.Combi == 4)
            {
                EditarPlatoView epv = new EditarPlatoView(_, idmesa);
                await Navigation.PushModalAsync(epv);
            }
            else
            {
                await DisplayAlert("Informacion", "El articulo no es combinado", "Aceptar");
            }
        }
        private async void ButtonNotas_Clicked(object sender, EventArgs e)
        {
            var _ = (ClienteModel)(sender as MenuItem).CommandParameter;
            if (_.print == 0)
            {
                if (string.IsNullOrEmpty(_.notas))
                {
                    var result = await DisplayPromptAsync(_.Plato, "Nota", keyboard: Keyboard.Chat, maxLength: 60);
                    if (!string.IsNullOrEmpty(result))
                    {
                        _.notas = result;
                        fvm.Actualizarnota(_.Lastid, result);
                        ActualizarVista();
                    }
                }
                else
                {
                    await DisplayAlert("Información", "El articulo ya contiene una nota", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Información", "La comanda ya esta Impresa", "Aceptar");
            }
        }
        private async void CambiarMesa_Clicked(object sender, EventArgs e)
        {
            try
            {
                //ClienteModel _ = (ClienteModel)(sender as MenuItem).CommandParameter;
                if (Clientes.Count > 0)
                {
                    var _ = pmvm.Mesasdisponibles(idmesa);
                    if (_.Count > 0)
                    {
                        string result = await DisplayActionSheet("Mesas disponibles", "Cancelar", null, _.Select(device => device.Mes_numero).ToArray());
                        if (!string.IsNullOrEmpty(result) && !result.Equals("Cancelar") && !result.Equals(mesnumero))
                        {
                            foreach (var item in _)
                            {
                                if (item.Mes_numero.Equals(result))
                                {
                                    var popupProperties = new CambioMesaView(Clientes, item, Recalcularvalor());
                                    var scaleAnimation = new ScaleAnimation
                                    {
                                        PositionIn = MoveAnimationOptions.Right,
                                        PositionOut = MoveAnimationOptions.Left
                                    };
                                    popupProperties.Animation = scaleAnimation;
                                    popupProperties.CloseWhenBackgroundIsClicked = true;
                                    await PopupNavigation.Instance.PushAsync(popupProperties);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Información", "Lista de articulos vacia ", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex, "Aceptar");
            }

        }
        private async void CombinarMesas_Clicked(object sender, EventArgs e)
        {
            try
            {
                var popupProperties = new CombinarMesasView(mesa, idmesareferenciada);
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = MoveAnimationOptions.Right,
                    PositionOut = MoveAnimationOptions.Left
                };
                popupProperties.Animation = scaleAnimation;
                popupProperties.CloseWhenBackgroundIsClicked = true;
                await PopupNavigation.Instance.PushAsync(popupProperties);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }
        protected override bool OnBackButtonPressed()
        {

            //PopupNavigation.Instance.PopAsync();
            return base.OnBackButtonPressed();
        }
        private void ActualizarVista()
        {

            Clientes.Clear();
            PedidosMesasViewMode.ConsultaPedidosMesas(idmesa);
            pmvm = PedidosMesasViewMode;
            try
            {
                if (PedidosMesasViewMode.PedidosMesas[0].Mes_estado == 2)
                {
                    List<string> nit = new List<string>();
                    foreach (var item in PedidosMesasViewMode.PedidosMesas)
                    {
                        if (item.Mvm_combi == 4)
                        {
                            nit.Add(item.Mvm_nit);
                            AgregarArticulo(ArticulosReturn(item));
                        }
                    }
                    foreach (var item in PedidosMesasViewMode.PedidosMesas)
                    {
                        if (item.Mvm_combi != 4)
                        {
                            AdicionalXcliente = true;
                            foreach (var itemnit in nit)
                            {
                                if (item.Mvm_nit.Equals(itemnit))
                                {
                                    AdicionalXcliente = false;
                                    break;
                                }
                            }
                            if (AdicionalXcliente)
                            {
                                AgregararticuloInde(ArticulosReturn(item));
                                AdicionalXcliente = true;
                            }
                        }
                    }
                }
                else
                {
                    if (ActivacionArticulos)
                    {
                        VistaArticulo();
                        ActivacionArticulos = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "" + ex, "Aceptar");
            }
        }


    }
}