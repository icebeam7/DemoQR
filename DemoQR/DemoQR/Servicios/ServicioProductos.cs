using DemoQR.Models;
using System.Linq;
using System.Collections.Generic;

namespace DemoQR.Servicios
{
    public class ServicioProductos
    {
        public static Producto ObtenerProducto(string id)
        {
            return productos.FirstOrDefault(x => x.Id == id);
        }

        private static List<Producto> productos = new List<Producto>()
        {
            new Producto() { Id = "1", Nombre = "Galletas", Precio = 100 },
            new Producto() { Id = "2", Nombre = "Leche", Precio = 200 },
            new Producto() { Id = "3", Nombre = "Jugo", Precio = 300 },
            new Producto() { Id = "4", Nombre = "Huevos", Precio = 400 },
            new Producto() { Id = "5", Nombre = "Telefono", Precio = 500 },
        };
    }
}
