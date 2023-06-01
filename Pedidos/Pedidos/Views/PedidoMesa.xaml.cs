using PedidosRestaurante.ViewsModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidoMesa : ContentPage
    {

        //readonly PedidosMesasViewModel PedidosMesasViewMode;
        //readonly int id_mesa;
        //private readonly string mes_numero;
        //private readonly string fechaingreso;
        //readonly List<XxxxartiModel> Lista_factura = new List<XxxxartiModel>();
        public PedidoMesa(int id_mes, string mes_num, PedidosMesasViewModel pedidos)
        {
            //mes_numero = mes_num;
            //id_mesa = id_mes;
            //PedidosMesasViewMode = pedidos;

            //InitializeComponent();
            //decimal valorall = 0.0m;
            //foreach (var item in PedidosMesasViewMode.PedidosMesas)
            //{
            //    valorall += item.Mvm_cant * item.Artivlr1_c;
            //}
            //valormesa.Text = string.Format("{0:n0}", valorall);
            //BindingContext = PedidosMesasViewMode;
            //fechaingreso = PedidosMesasViewMode.Fechaingreso;
        }

        private void Buttoneliminar_Clicked(object sender, EventArgs e)
        {
            //bool result = await DisplayAlert("Eliminar Comanda", "¿Desea elimnar la comanda?", "Si", "No");
            //if (result)
            //{
            //    //PedidosMesasViewMode.Eliminarcomanda(id_mesa);
            //    //Application.Current.MainPage = new NavigationPage(new CategoriasTabbedPage());
            //}
        }

        private void ButtonPedidos_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new PedidosComanda(id_mesa, mes_numero, PedidosMesasViewMode.PedidosMesas));
        }

        private void Buttonprinter_Clicked(object sender, EventArgs e)
        {
            //Lista_factura.Clear();
            //foreach (var item in PedidosMesasViewMode.PedidosMesas)
            //{
            //    Lista_factura.Add(new XxxxartiModel
            //    {
            //        Artinomb = item.Mvm_nombre,
            //        Articantidad = (int)item.Mvm_cant,
            //        Artivalor = string.Format("{0:n0}", item.Mvm_valor),
            //        Id_mesa = id_mesa,
            //        Mvm_mesa = mes_numero,
            //        Mvm_codigo = item.Mvm_codigo,
            //        Mvm_valor = string.Format("{0:n0}", item.Mvm_neto),
            //        Mvm_unidad = item.Mvm_unidad,
            //        Mvm_grupo = item.Mvm_grupo,
            //        Mvm_notas = item.Mvm_notas,
            //        Artxctoult = 0.0m,
            //        Artivlr1_c = item.Artivlr1_c,
            //        Mvm_neto = item.Mvm_neto,
            //        Artiiva = 0,
            //        Id_mesm = item.Id_mesm,
            //        Tipo_mesa = item.Mes_tipo
            //    });
            //}
            //var printer = DependencyService.Get<PedidosRestaurante.IPrinter>();
            //var ip = await App.Context.GetprinterAsync();
            //if (printer == null)
            //{
            //    await DisplayAlert("ERROR", "No implemnetacion", "Aceptar");
            //    return;
            //}
            //try
            //{                
            //    string ipAnddress = ip[0].Ip;                
            //    int portNmber = 9100;
            //    //printer.Print(ipAnddress, portNmber, Lista_factura);
            //    await DisplayAlert("Informativo", "Esta imprimiendo", "Aceptar");
            //    ////Application.Current.MainPage = new CategoriasTabbedPage(DateTime.Now.AddDays(5).ToString(), DateTime.Now.AddDays(10));
            //}
            //catch (Exception)
            //{                
            //    var result = await DisplayPromptAsync("ERROR", "Digite la IP de la impresora");
            //    if (ip.Count == 0)
            //    {

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ImpresoraModel impresora = new ImpresoraModel
            //            {
            //                Ip = result
            //            };
            //            await App.Context.InsertprinterAsync(impresora);
            //        }
            //    }
            //    else
            //    {
            //        ImpresoraModel impresora = new ImpresoraModel
            //        {
            //            Id = 1,
            //            Ip = result
            //        };
            //        await App.Context.UpdateprinterAsync(impresora);
            //    }

            //}
        }

        private void Buttoncambiarmesa_Clicked(object sender, EventArgs e)
        {
            //await PedidosMesasViewMode.Mesasdisponibles();
            //string result = await DisplayActionSheet("Mesas disponibles", "Cancelar", null, this.PedidosMesasViewMode.Mesasfree.Select(device => device.Mes_numero).ToArray());
            //foreach (var item in PedidosMesasViewMode.Mesasfree)
            //{
            //    if (item.Mes_numero.Equals(result))
            //    {
            //        PedidosMesasViewMode.Actualizarmesas(result, Convert.ToDecimal(PedidosMesasViewMode.Valormesa), mes_numero, item.Id_mesa, fechaingreso);
            //        break;
            //    }
            //}
            //Application.Current.MainPage = new NavigationPage(new CategoriasTabbedPage());
        }


    }
}