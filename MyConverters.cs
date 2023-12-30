using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Data;
using Model;

namespace WpfLibrary
{
    public class MyConverters
    {
    }

    public static class ByteImageConverter
    {

        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);

            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
  
            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;

        }

    }


    public class FullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Users user)
            {
                return $"{user.FirstName} {user.LastName}";
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }



}
