using DemoQR.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerProductosView : ContentPage
    {
        ScannerProductosViewModel vm;
        
        public ScannerProductosView()
        {
            InitializeComponent();

            vm = new ScannerProductosViewModel(Navigation);
            BindingContext = vm;
        }
    }
}