using System.Windows.Input;
using System.Threading.Tasks;

using DemoQR.Interfaces;

using ZXing;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace DemoQR.ViewModels
{
    public class GenerarQRViewModel : BaseViewModel
    {
        private string codigo;

        public string Codigo
        {
            get => codigo;
            set
            {
                codigo = value;
                OnPropertyChanged();
            }
        }

        private string codigoConvertido;

        public string CodigoConvertido
        {
            get => codigoConvertido;
            set { codigoConvertido = value; OnPropertyChanged(); }
        }

        public ICommand ComandoGenerarCodigo { get; private set; }
        public ICommand ComandoGuardar { get; private set; }
        public ICommand ComandoCompartir { get; private set; }

        private string RutaImagen;
        private BarcodeFormat formatoSeleccionado = BarcodeFormat.QR_CODE;

        private void GenerarCodigo()
        {
            CodigoConvertido = codigo;
        }

        private async Task Guardar()
        {
            IImagen ic = DependencyService.Get<IImagen>();
            RutaImagen = await ic.Guardar(CodigoConvertido, formatoSeleccionado, 400, 400);
            await App.Current.MainPage.DisplayAlert("Listo", "Archivo guardado", "OK");
        }

        private  async Task Compartir()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Producto " + Codigo,
                File = new ShareFile(RutaImagen)
            });
        }

        public GenerarQRViewModel()
        {
            ComandoGenerarCodigo = new Command(GenerarCodigo);
            ComandoGuardar = new Command(async () => await Guardar());
            ComandoCompartir = new Command(async () => await Compartir());

            Codigo = "-----";
            codigoConvertido = "-----";
        }
    }
}
