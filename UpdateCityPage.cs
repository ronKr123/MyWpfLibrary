using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApiLibraryService;

namespace WpfLibrary
{
    public class UpdateCityPage : AddCityPage
    {
        //ApiService api = new ApiService();

        public static City CityParameter { get; set; }


        // לפני המעבר לדף זה , צריך להתחל את הפרמטר city לדף
        // UpdateCityPage.CityParameter = city...;

        public UpdateCityPage(CityList cities) : base(cities)
        {
            this.TitlePageCity.Text = "עדכון עיר";
            this.addCity.Content = "עדכון";
            if(CityParameter != null)
            {
                this.cityNameTxtBox.Text = CityParameter.CityName;
            }
        }


        public override async void addCity_Click(object sender, RoutedEventArgs e)
        {
            City cityUpdated = CityParameter;

            string cityName = this.cityNameTxtBox.Text;

            if ((cityName != null) && (!ExistCity(cityName)))
            {
                cityUpdated.CityName = cityName;
                int x = await api.UpdateACity(cityUpdated);
                if (x != 0)
                {
                    MessageBox.Show("העיר עודכנה בהצלחה");
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
                if (cityName == null)
                {
                    MessageBox.Show("לא הוכנסו נתונים");
                }
            }
        }



        //Checking if there is a city with the same name, (if Exist)

        private bool ExistCity(string cityName)
        {
            CityList cities = CitiesAllList;
            foreach (City city in cities)
            {
                if (city.CityName.Equals(cityName))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
