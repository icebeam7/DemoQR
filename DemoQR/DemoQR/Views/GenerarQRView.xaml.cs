using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DemoQR.ViewModels;

namespace DemoQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerarQRView : ContentPage
    {
        GenerarQRViewModel vm;

        public GenerarQRView()
        {
            InitializeComponent();

            vm = new GenerarQRViewModel();
            BindingContext = vm;
        }
    }
}