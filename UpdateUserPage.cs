using ApiLibraryService;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfLibrary
{
    public class UpdateUserPage: PagePerson
    {

        public static Users Userr { get; set; }

        public UpdateUserPage(Model.CityList cities) : base()
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
            this.NamePage.Text = "עדכון פרטים אישיים";
            this.ToLoginPage.Visibility = System.Windows.Visibility.Collapsed;

            Users user = Userr;


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

            int i = 0;
            string cityUser = Userr.CityCode.CityName;

            foreach(City city in cities)
            {
                if (city.CityName.Equals(cityUser))
                {
                    this.comboBoxCity.SelectedIndex = i;
                }
                i++;
            }

            string numPhoneAfterPre = user.PhoneNumber;
            string[] words = numPhoneAfterPre.Split('-');
            this.numPhoneAfterPreTxtBox.Text = words[1].ToString();

            int j = 0;
            string prePhoneUser = words[0];

            foreach(string prePhone in prePhonesNumbers) 
            {
                if (prePhone.Equals(prePhonesNumbers[j]))
                {
                    this.prePhone.SelectedIndex = j;
                }
                j++;
            }

            this.firstNameTxtBox.Text = user.FirstName;
            this.lastNameTxtBox.Text = user.LastName;
            this.datePicker.Text = user.DateOfBirth.ToString();
            this.userNameTxtBox.Text = user.UserName;
            this.passWordTxtBox.Text = user.UserPassword;
            this.emailTxtBox.Text = user.Email;
      

        }

        public async override void Send(object sender, System.Windows.RoutedEventArgs e)
        {

            Users user = Userr;

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
            foreach (City city in cities)
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

            int num = await api.UpdateAUser(user);

            if (num != 0)
            {
                MessageBox.Show("המשתמש עודכן בהצלחה");
            }
            else
            {
                MessageBox.Show("שגיאה");
            }

        }
    }
}
