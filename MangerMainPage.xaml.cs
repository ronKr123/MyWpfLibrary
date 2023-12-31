using ApiLibraryService;
using Model;
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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for MangerMainPage.xaml
    /// </summary>
    public partial class MangerMainPage : Page
    {
        ApiService api = new ApiService();


        public MangerMainPage()
        {
            InitializeComponent();
            FrameHolder.MangerFrame = this.frameManger;

            this.frameManger.Navigate(new ScreenViewManger());

        }


        private void ToHomeMangerPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new ScreenViewManger());

        }


        private void ToAllUseresPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new UsersListPage());

        }


        private void ToAddNewBookPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new AddingNewBookPage());

        }


        private void ToAddingDigitalBookPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new AddNewDigitalBookPage());
        }

        private void ToAllCitiesPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new CityListPage());

        }

        private void ToAllGenresPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new GenresListPage());

        }

        private void ToAllWritersPage(object sender, MouseButtonEventArgs e)
        {
            this.frameManger.Navigate(new WritesList());

        }

        private async void ToAllBooksPage(object sender, MouseButtonEventArgs e)
        {
            BooksList books = await api.SelectAllBooks();
            this.frameManger.Navigate(new PageListBooks(books));

        }

        private async void ToAllAudioBooksPage(object sender, MouseButtonEventArgs e)
        {
            DigitalBooksList digitalBooks = await api.SelectAllDigitalBooks();
            this.frameManger.Navigate(new PageDigitalBooks(digitalBooks));

        }

        private async void ToNewLendingPage(object sender, MouseButtonEventArgs e)
        {
            BooksList books = await api.SelectAllBooks();
            UsersList users = await api.SelectAllUsers();

            this.frameManger.Navigate(new LendingBooksPage(books, users));

        }

        private async void ToReturnBookPage(object sender, MouseButtonEventArgs e)
        {
            LendingAndReturnsBooksList lendingAndReturns = await api.SelectAllLendingAndReturnsBooks();
            this.frameManger.Navigate(new ReturnBooksPage(lendingAndReturns));

        }

    }
}
