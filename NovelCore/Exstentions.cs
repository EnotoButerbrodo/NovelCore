using System.IO;
using System.Windows.Media.Imaging;

namespace NovelCore
{
    public static class Exstentions
    {
        public static BitmapImage toBitmapImage(this MemoryStream stream)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = stream;
            src.EndInit();
            return src;
        }
    }
}
