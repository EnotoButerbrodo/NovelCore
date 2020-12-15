using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;

namespace NovelCore
{
    public class SceneImage
    {
        public SceneImage(Image image)
        {
            Image = image;
        }
        Image Image { get; set; } = new Image();
        public void Scale(double x, double y)
        {     
            ScaleTransform scale = new ScaleTransform(x, y);
            Image.RenderTransform = scale;
        }
    }
}
