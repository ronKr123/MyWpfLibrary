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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for LendAndRetUC.xaml
    /// </summary>
    public partial class LendAndRetUC : UserControl
    {
        ApiService apiService = new ApiService();

        public static DigitalBooksList ListDigitalBooks { get; set; }


        public LendAndRetUC(Model.LendingAndReturnsBooks lendAndRetBook)
        {
            InitializeComponent();
            ImageFromDataBase(lendAndRetBook.BookCode.Id);
            this.bookName.Text = lendAndRetBook.BookCode.BookName;
            string dateLending = lendAndRetBook.DateOfLending.ToString("d/M/yyyy");
            string dateReturn = lendAndRetBook.DateOfReturn.ToString("d/M/yyyy");
            this.dateLendingBookTxt.Text = dateLending;
            this.dateReturnBookTxt.Text = dateReturn;

            this.digitalBookTxt.Visibility = Visibility.Collapsed;
            ////GetDigitalBooksList();
            ////ChangeTypeBook(lendAndRetBook.BookCode, ListDigitalBooks);

        }

        public async void GetDigitalBooksList()
        {
            DigitalBooksList digitalBooks = await apiService.SelectAllDigitalBooks();
            ListDigitalBooks = digitalBooks;
        }


        public void ChangeTypeBook(Books book, DigitalBooksList digitalBooks)
        {
            int id = book.Id;
            foreach (DigitalBooks digiBook in digitalBooks)
            {
                if (digiBook.Id == id)
                {
                    this.digitalBookTxt.Visibility = Visibility.Visible;
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
            this.imgBook.Source = ByteImageConverter.ByteToImage(imgStr);
        }


    }
}
