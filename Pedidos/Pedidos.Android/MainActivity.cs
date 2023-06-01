using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(PedidosRestaurante.Droid.MainActivity))]


namespace PedidosRestaurante.Droid
{
    [Activity(Label = "PedidosRestaurantes", Icon = "@drawable/icono", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IDevice
    {
        public static ContentResolver myContentResolver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            myContentResolver = this.ContentResolver;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Rg.Plugins.Popup.Popup.Init(this);
            UserDialogs.Init(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [System.Obsolete]
        public string DeviceID()
        {
            string deviceID = Build.Serial?.ToString();
            if (string.IsNullOrEmpty(deviceID) || deviceID.ToUpper() == "UNKNOWN") // Android 9 returns "Unknown"
            {
                ContentResolver myContentResolver = MainActivity.myContentResolver;
                deviceID = Secure.GetString(myContentResolver, Secure.AndroidId);
            }
            return deviceID;
        }
    }
}