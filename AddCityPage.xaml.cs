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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for AddCityPage.xaml
    /// </summary>
    public partial class AddCityPage : Page
    {
        public static ApiService api = new ApiService();

        public static CityList CitiesAllList { get; set; }

        public AddCityPage(Model.CityList cities)
        {
            InitializeComponent();
            this.cityNameTxtBox.Text = "";
            if (cities != null)
            {
                CitiesAllList = cities;
            }

        }



        public virtual async void addCity_Click(object sender, RoutedEventArgs e)
        {
            string cityName = this.cityNameTxtBox.Text;
            if((cityName != null) && (!ExistCity(cityName)))
            {
                City newCity = new City();
                newCity.CityName = cityName;
                int x = await api.InsertACity(newCity);
                if(x != 0)
                {
                    MessageBox.Show("העיר הוספה בהצלחה");
                    this.cityNameTxtBox.Text = "";
                    CitiesAllList = await api.SelectAllCities();
                }
            }
            else
            {
                if (ExistCity(cityName))
                {
                    MessageBox.Show("כבר קיימת עיר בשם זה");
                }
                if(cityName == null)
                {
                    MessageBox.Show("לא הוכנסו נתונים");
                }
            }
        }


        //Checking if there is a city with the same name, (if Exist)

        private bool ExistCity(string cityName)
        {
            CityList cities = CitiesAllList;
            foreach(City city in cities)
            {
                if (city.CityName.Equals(cityName))
                {
                    return true;
                }
            }
            return false;
        }

        

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            NavigationService?.GoBack();
        }

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
