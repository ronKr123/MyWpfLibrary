using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ViewModel;
using ApiLibraryService;
using System.Windows;
using Windows.UI.Xaml;

namespace WpfLibrary
{
    public class userPage: PagePerson
    {
        //public static CityList cities;
        //public static Users user;

        public userPage(Model.CityList cities) : base()
        {
            //this.userNameTxtBox.Visibility = System.Windows.Visibility.Visible;

            this.txtCity.Visibility = System.Windows.Visibility.Visible;
            this.comboBoxCity.Visibility = System.Windows.Visibility.Visible;
            this.txtUserName.Visibility = System.Windows.Visibility.Visible;
            this.userNameTxtBox.Visibility = System.Windows.Visibility.Visible;
            this.txtPass.Visibility = System.Windows.Visibility.Visible;
            this.passWordTxtBox.Visibility = System.Windows.Visibility.Visible;
            this.txtEmail.Visibility = System.Windows.Visibility.Visible;
            this.emailTxtBox.Visibility = System.Windows.Visibility.Visible;
            this.txtPhoneNumber.Visibility = System.Windows.Visibility.Visible;
            this.txtSplitPhone.Visibility = System.Windows.Visibility.Visible;
            this.prePhone.Visibility = System.Windows.Visibility.Visible;
            this.NamePage.Text = "הרשמה";

            List<string> prePhonesNumbers = new List<string>();
            prePhonesNumbers.Add("050");
            prePhonesNumbers.Add("053");
            prePhonesNumbers.Add("054");
            prePhonesNumbers.Add("058");
            this.prePhone.ItemsSource = prePhonesNumbers;

            if (cities != null)
            {
                List<string> citiesListNames = new List<string>();
                foreach (City city in cities)
                {
                    citiesListNames.Add(city.CityName);
                }
                this.comboBoxCity.ItemsSource = citiesListNames;
            }

        }


        public async override void Send(object sender, System.Windows.RoutedEventArgs e)
        {
            //adding new user

            Users user = new Users();

            user.FirstName = this.firstNameTxtBox.Text;
            user.LastName = this.lastNameTxtBox.Text;
            
            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                user.DateOfBirth = (DateTime)selectedDate;

            }
            else
            {
                MessageBox.Show("בבקשה לבחור תאריך לידה");
                return;
            }

            String? citySelectedName = this.comboBoxCity.SelectedItem as string;

            CityList cities = await api.SelectAllCities();
            foreach(City city in cities)
            {
                if (city.CityName.Equals(citySelectedName))
                {
                    user.CityCode = city;
                }
            }



            user.UserName = this.userNameTxtBox.Text;
            user.UserPassword = this.passWordTxtBox.Text;
            user.Email = this.emailTxtBox.Text;

            String? prePhone = this.prePhone.SelectedItem as string;

            user.PhoneNumber = prePhone + "-" + this.numPhoneAfterPreTxtBox.Text;

            

            int num = await api.InsertAUser(user);

            if(num != 0)
            {
                MessageBox.Show("המשתמש התווסף בהצלחה");
                this.firstNameTxtBox.Text = "";
                this.lastNameTxtBox.Text = "";
                this.datePicker.Text = "";
                this.userNameTxtBox.Text = "";
                this.passWordTxtBox.Text = "";
                this.numPhoneAfterPreTxtBox.Text = "";
                this.prePhone.SelectedItem = null;
                this.emailTxtBox.Text = "";
                this.comboBoxCity.SelectedValue = null;

            }
        }

        public async Task<CityList> GetCitiesList()
        {
            ApiService api = new ApiService();
            CityList cities = await api.SelectAllCities();
            return cities;
        }




    }
}
