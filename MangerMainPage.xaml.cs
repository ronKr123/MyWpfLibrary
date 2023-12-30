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


        private async void ToAddNewCityPage(object sender, MouseButtonEventArgs e)
        {
            CityList cities = await api.SelectAllCities();
            this.frameManger.Navigate(new AddCityPage(cities));

        }


        private async void ToAddNewGenrePage(object sender, MouseButtonEventArgs e)
        {
            GenreList genres = await api.SelectAllGeneres();
            this.frameManger.Navigate(new AddGenrePage(genres));

        }

        private async void ToAddNewWriter(object sender, MouseButtonEventArgs e)
        {
            GenreList genres = await api.SelectAllGeneres();
            this.frameManger.Navigate(new InsertNewWriter(genres));

        }

        private async void ToAddNewUserPage(object sender, MouseButtonEventArgs e)
        {
            CityList cities = await api.SelectAllCities();
            userPage page = new userPage(cities);
            page.ToLoginPage.Visibility = Visibility.Collapsed;
            this.frameManger.Navigate(page);

        }


    }
}
