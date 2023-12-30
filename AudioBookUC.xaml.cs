using ApiLibraryService;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for AudioBookUC.xaml
    /// </summary>
    public partial class AudioBookUC : UserControl
    {
        ApiService api = new ApiService();


        public AudioBookUC(Model.DigitalBooks digitalBook)
        {
            InitializeComponent();
            this.bookName.Text = digitalBook.BookName;
            ImageFromDataBase(digitalBook.Id);

        }


        private async void ImageFromDataBase(int idBook)
        {
            await DoJob(idBook);

        }

        private async Task DoJob(int idBook)
        {
            // מקבלת את המחרוזת 64 של אותו ספר ספציפי
            // הופכת את הסטרינג הארוך מאוד לביתים , ואותם ממירה לתמונה
            // שאת התמונה מוסיפים ל - source  

            ApiService apiService = new ApiService();
            string st = await apiService.GetBookPicByte64(idBook);
            byte[] imgStr = Convert.FromBase64String(st);
            this.digiBookImage.Source = ByteImageConverter.ByteToImage(imgStr);
        }


        private async void btnToAudioBookPage_Click(object sender, RoutedEventArgs e)
        {
            DigitalBooksList digitalBooks = await api.SelectAllDigitalBooks();
            DigitalBooks digitalBook = digitalBooks.Find(item => item.BookName.Equals(this.bookName.Text));
            if(digitalBook != null)
            {
                string audioFileName = digitalBook.BookAudioFile;

                Frame frame = FrameHolder.MainFrameUser;
                Frame frameManger = FrameHolder.MangerFrame;
                frame.Navigate(new AudioBookPage(digitalBook, audioFileName));
                if (frameManger != null)
                {
                    frameManger.Navigate(new AudioBookPage(digitalBook, audioFileName));
                }

            }
        }


    }
}
