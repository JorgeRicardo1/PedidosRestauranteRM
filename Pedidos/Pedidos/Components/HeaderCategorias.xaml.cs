
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PedidosRestaurante.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderCategorias : ContentView
    {
        public HeaderCategorias()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty MesasocupadasProperty = BindableProperty.Create(
        "Mesasocupadas",        // the name of the bindable property
        typeof(int),     // the bindable property type
        typeof(int));  // the parent object type


        public int Mesasocupadas
        {
            get => (int)GetValue(MesasocupadasProperty); set => SetValue(MesasocupadasProperty, value);
        }

        public static readonly BindableProperty MesaslibresProperty = BindableProperty.Create(
       "Mesaslibres",        // the name of the bindable property
       typeof(int),     // the bindable property type
       typeof(int));  // the parent object type


        public int Mesaslibres
        {
            get => (int)GetValue(MesaslibresProperty); set => SetValue(MesaslibresProperty, value);
        }
    }
}