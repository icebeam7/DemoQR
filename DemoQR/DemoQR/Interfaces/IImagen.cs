using System.Threading.Tasks;
using ZXing;

namespace DemoQR.Interfaces
{
    public interface IImagen
    {
        Task<string> Guardar(string codigo, BarcodeFormat formato, int ancho, int altura);
    }
}
