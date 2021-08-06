using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

using Xamarin.Forms;

using DemoQR.Models;
using DemoQR.Servicios;

namespace DemoQR.ViewModels
{
    public class ScannerProductosViewModel : BaseViewModel
    {
        private string resultado;

        public string Resultado
        {
            get => resultado;
            set { resultado = value; OnPropertyChanged(); }
        }

        private Producto producto;

        public Producto Producto
        {
            get { return producto; }
            set { producto = value; OnPropertyChanged(); }
        }

        private string imagen1;

        public string Imagen1
        {
            get => imagen1;
            set { imagen1 = value; OnPropertyChanged(); }
        }

        public ICommand ComandoEscanear { get; protected set; }

        private INavigation Navegacion { get; set; }

        private void ObtenerResultado(Result escaneo)
        {
            Resultado = escaneo.Text;
        }

        private async Task Escanear()
        {
            var opciones = new MobileBarcodeScanningOptions();

            opciones.PossibleFormats = new List<BarcodeFormat>()
            {
                BarcodeFormat.QR_CODE
            };

            var overlay = new ZXingDefaultOverlay()
            {
                ShowFlashButton = true,
                TopText = "Coloca el código frente al dispositivo",
                BottomText = "Escaneo automatico",
                Opacity = 0.75
            };

            overlay.BindingContext = overlay;

            var pagina = new ZXingScannerPage(opciones, overlay)
            {
                Title = "Escaneo de código",
                BackgroundColor = Color.LightBlue
            };

            await Navegacion.PushAsync(pagina);

            pagina.OnScanResult += (codigo) =>
            {
                pagina.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navegacion.PopAsync();
                    ObtenerResultado(codigo);

                    Producto = ServicioProductos.ObtenerProducto(Resultado);
                });
            };
        }

        public ScannerProductosViewModel(INavigation navegacion)
        {
            this.Navegacion = navegacion;
            resultado = "------";
            ComandoEscanear = new Command(async () => await Escanear());

            Imagen1 = "/storage/emulated/0/Android/data/com.companyname.demoqr/files/Pictures/codigos/470f06c4-9713-43bb-990d-1c05cb80bbd6.png";
        }
    }
}