using Android.App;
using Android.OS;

namespace PedidosRestaurante.Droid
{
    [Activity(Label = "OrdersApp", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
            OverridePendingTransition(0, 0);
        }
    }
}