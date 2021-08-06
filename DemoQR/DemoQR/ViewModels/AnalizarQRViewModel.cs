using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Essentials;

using DemoQR.Models;
using DemoQR.Servicios;

namespace DemoQR.ViewModels
{
    public class AnalizarQRViewModel : BaseViewModel
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

        public ICommand ComandoElegir { get; protected set; }

        private async Task Elegir()
        {
            var foto = await MediaPicker.PickPhotoAsync();
            await CopiarAnalizarImagen(foto);
        }

        private async Task CopiarAnalizarImagen(FileResult foto)
        {
            if (foto != null)
            {
                byte[] bytes = null;

                var rutaCopia = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

                using (var streamFoto = await foto.OpenReadAsync())
                {
                    using (var streamCopia = File.Open(rutaCopia, FileMode.OpenOrCreate))
                    {
                        await streamFoto.CopyToAsync(streamCopia);

                        streamCopia.Seek(0, SeekOrigin.Begin);
                        bytes = new byte[streamCopia.Length];
                        streamCopia.Read(bytes, 0, bytes.Length);
                    }
                }

                Imagen1 = rutaCopia;

                if (bytes != null)
                {
                    List<GoogleVisionBarCodeScanner.BarcodeResult> analisis = await GoogleVisionBarCodeScanner.Methods.ScanFromImage(bytes);
                    var qr = analisis.FirstOrDefault();

                    if (qr != null)
                    {
                        Resultado = qr.DisplayValue;
                        Producto = ServicioProductos.ObtenerProducto(Resultado);
                    }
                }
            }
            else
            {
                Imagen1 = null;
            }
        }

        public AnalizarQRViewModel()
        {
            resultado = "------";
            ComandoElegir = new Command(async () => await Elegir());
        }
    }
}
