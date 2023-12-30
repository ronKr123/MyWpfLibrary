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
    /// Interaction logic for LoginOrRegisterPage.xaml
    /// </summary>
    public partial class LoginOrRegisterPage : Page
    {
        private NavigationService navSrv;

        public LoginOrRegisterPage()
        {
            InitializeComponent();
            this.Loaded += Login_Loaded;

        }


        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            this.navSrv = this.NavigationService;
            this.navSrv.Navigating += NavSrv_Navigating; ;
        }

        private void NavSrv_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
                e.Cancel = true;
        }


        private void login_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());

        }

        private async Task register_ClickAsync(object sender, RoutedEventArgs e)
        {
            ApiService apiService = new ApiService();
            CityList cities = await apiService.SelectAllCities();

            this.NavigationService.Navigate(new userPage(cities));
        }

        private async void register_Click(object sender, RoutedEventArgs e)
        {
            ApiService apiService = new ApiService();
            CityList cities = await apiService.SelectAllCities();

            this.NavigationService.Navigate(new userPage(cities));
        }

        private async void register_Click_1(object sender, RoutedEventArgs e)
        {
            ApiService apiService = new ApiService();
            CityList cities = await apiService.SelectAllCities();

            this.NavigationService.Navigate(new userPage(cities));

        }

        private void WantToExit_Click()
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("?אתם רוצים לצאת", "",
                                  MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response != MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
