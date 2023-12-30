using ApiLibraryService;
using System;
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
using Model;
using ViewModel;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for BookPage.xaml
    /// </summary>
    public partial class BookPage : Page
    {
        public static Books PublicVariableBook { get; set; }

        ApiService apiService = new ApiService();

        public BookPage()
        {
            InitializeComponent();
            Books bookView = PublicVariableBook;

            this.digiBookText.Visibility = Visibility.Collapsed;
            this.durtionTxt.Visibility = Visibility.Collapsed;
            this.NumMinutesDurartionTxt.Visibility = Visibility.Collapsed;
            this.minutesTxt.Visibility = Visibility.Collapsed;

            ImageFromDataBase(bookView.Id);
            this.bookName.Text = bookView.BookName;
            this.genreBookName.Text = bookView.GenreCode.GenreName;
            this.writerBookName.Text = bookView.WriterCode.FirstName + " " + bookView.WriterCode.LastName;


            IfDigitalBook(bookView);

        }

        private async void IfDigitalBook(Books bookView)
        {
            DigitalBooksList digitalBooksList = await apiService.SelectAllDigitalBooks();
            foreach(DigitalBooks digitalBook in digitalBooksList)
            {
                if(digitalBook.Id == bookView.Id)
                {
                    this.digiBookText.Visibility = Visibility.Visible;
                    this.durtionTxt.Visibility = Visibility.Visible;
                    this.NumMinutesDurartionTxt.Text = digitalBook.Duration.ToString();
                    this.NumMinutesDurartionTxt.Visibility = Visibility.Visible;
                    this.minutesTxt.Visibility = Visibility.Visible;
                }
            }
        }



        private async void ImageFromDataBase(int idBook)
        {
            await DoJob(idBook);
        }

        private async Task DoJob(int idBook)
        {
            string st = await apiService.GetBookPicByte64(idBook);
            byte[] imgStr = Convert.FromBase64String(st);
            this.imgDb.Source = ByteImageConverter.ByteToImage(imgStr);

        }


        private void ToBackPage_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            NavigationService?.GoBack();
        }



        // Get the NavigationService from the hosting window
        private NavigationService NavigationService
        {
            get
            {
                if (NavigationService.GetNavigationService(this) is NavigationService navigationService)
                    return navigationService;

                return null;
            }
        }


    }
}
