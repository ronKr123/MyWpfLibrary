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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for PagePerson.xaml
    /// </summary>
    public partial class PagePerson : Page
    {

        public static ApiService api = new ApiService();

        public PagePerson()
        {
            InitializeComponent();
            this.txtCity.Visibility = Visibility.Collapsed;
            this.comboBoxCity.Visibility = Visibility.Collapsed;
            this.txtUserName.Visibility = Visibility.Collapsed;
            this.userNameTxtBox.Visibility = Visibility.Collapsed;
            this.txtPass.Visibility = Visibility.Collapsed;
            this.passWordTxtBox.Visibility = Visibility.Collapsed;
            this.txtEmail.Visibility = Visibility.Collapsed;
            this.emailTxtBox.Visibility = Visibility.Collapsed;
            this.txtPhoneNumber.Visibility = Visibility.Collapsed;
            this.txtSplitPhone.Visibility = Visibility.Collapsed;
            this.prePhone.Visibility = Visibility.Collapsed;
            this.errorCity.Visibility = Visibility.Collapsed;
           
        }

       

        public virtual void Send(object sender, RoutedEventArgs e)
        {
            this.txtCity.Visibility = Visibility.Collapsed;
            MessageBox.Show("נשלח");

        }

        private void ToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void firstNameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

      
    }
}
