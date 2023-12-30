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
using ApiLibraryService;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page
    {
        ApiService apiService = new ApiService();



        public LoginPage()
        {
            InitializeComponent();
            this.PassWordTextBox.Password = "";
            this.uNTextBox.Text = "";
            loadingIndicator.Visibility = Visibility.Collapsed;


        }




        private async void submitLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.uNTextBox.Text.ToString() != null && this.PassWordTextBox.Password.ToString() != null)
            {
                string userName = this.uNTextBox.Text.ToString();
                string passUser = this.PassWordTextBox.Password.ToString();

                // Show the rotating indicator
                loadingIndicator.Visibility = Visibility.Visible;

                // Introduce a delay of 3 seconds
                await Task.Delay(3000);

                Users userFound = await apiService.SelectUserByUserName(userName);

                if (userFound == null)
                {
                    loadingIndicator.Visibility = Visibility.Collapsed;
                    MessageBox.Show("שם משתמש או סיסמה לא נכונים");
                }
                else 
                {
                    if (userFound.UserPassword.Equals(this.PassWordTextBox.Password.ToString()))
                    {
                        MainPage.Userr = userFound;
                        this.NavigationService.Navigate(new MainPage());
                        loadingIndicator.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }


        //מילוי נתונים אוטומטי בלחיצת כפתור מילוי
        private void fiil_Click(object sender, RoutedEventArgs e)
        {
            this.uNTextBox.Text = "dani10";
            this.PassWordTextBox.Password = "dani55";
        }

       

        private async void toRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            ApiService apiService = new ApiService();
            CityList cities = await apiService.SelectAllCities();

            this.NavigationService.Navigate(new userPage(cities));

        }

        private async void MangerLogin(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("? תרצה לעבור לכניסת מנהל", "",
                                  MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response != MessageBoxResult.No)
            {
                // Show the rotating indicator
                loadingIndicator.Visibility = Visibility.Visible;

                // Introduce a delay of 3 seconds
                await Task.Delay(3000);

                this.NavigationService.Navigate(new MangerMainPage());
                loadingIndicator.Visibility = Visibility.Collapsed;

            }
        }


       

        

       
    }
}
