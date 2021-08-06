using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

using DemoQR.Interfaces;
using DemoQR.Droid.Clases;

using Java.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

using ZXing;
using ZXing.Mobile;

[assembly: Dependency(typeof(ImagenAndroid))]
namespace DemoQR.Droid.Clases
{
    public class ImagenAndroid : IImagen
    {
        public async Task<string> Guardar(string codigo, BarcodeFormat formato, int ancho, int altura)
        {
            var options = new ZXing.Common.EncodingOptions()
            {
                Width = ancho,
                Height = altura
            };

            var writer = new BarcodeWriter()
            {
                Format = formato,
                Options = options
            };

            var folder = System.IO.Path.Combine(
                Android.App.Application.Context.GetExternalFilesDir(
                    Environment.DirectoryPictures).AbsolutePath, "codigos");

            Directory.CreateDirectory(folder);

            var name = $"{System.Guid.NewGuid()}.png";
            var filename = System.IO.Path.Combine(folder, name);

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                var bmp = writer.Write(codigo);
                bmp.Compress(Bitmap.CompressFormat.Png, 100, ms);
                ms.Seek(0, SeekOrigin.Begin);
                bytes = ms.ToArray();
            }

            using (var fos = new FileOutputStream(filename))
            {
                await fos.WriteAsync(bytes);
            }

            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
            {

                try
                {
                    var values = new ContentValues();
                    values.Put(MediaStore.Audio.Media.InterfaceConsts.DisplayName, System.IO.Path.GetFileNameWithoutExtension(filename));
                    values.Put(MediaStore.Audio.Media.InterfaceConsts.MimeType, "image/png");
                    values.Put(MediaStore.Images.Media.InterfaceConsts.Description, string.Empty);
                    values.Put(MediaStore.Images.Media.InterfaceConsts.DateTaken, Java.Lang.JavaSystem.CurrentTimeMillis());
                    values.Put(MediaStore.Images.ImageColumns.BucketId, filename.ToLowerInvariant().GetHashCode());
                    values.Put(MediaStore.Images.ImageColumns.BucketDisplayName, name.ToLowerInvariant());

                    var cr = Android.App.Application.Context.ContentResolver;
                    var albumUri = cr.Insert(MediaStore.Images.Media.ExternalContentUri, values);

                    using (System.IO.Stream input = System.IO.File.OpenRead(filename))
                    {
                        using (System.IO.Stream output = cr.OpenOutputStream(albumUri))
                        {
                            input.CopyTo(output);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
            }

            var uri = Uri.FromFile(new Java.IO.File(filename));
            return uri.Path;
        }
    }
}