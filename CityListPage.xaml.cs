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
using ApiLibraryService;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for CityListPage.xaml
    /// </summary>
    public partial class CityListPage : Page
    {
        ApiService apiService = new ApiService();

        private City selectedCity;

        public CityListPage()
        {
            InitializeComponent();
            LoadCitiesAsync();

        }


        private async void LoadCitiesAsync()
        {
            List<City> cities = await GetCities();
            //DataContext = userList;
            this.myListView.ItemsSource = cities;
        }

        private async Task<List<City>> GetCities()
        {
            CityList cities = await apiService.SelectAllCities();
            return cities.ToList(); 
        }


        private async void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCity != null)
            {

                CityList cities = await apiService.SelectAllCities();

                UpdateCityPage.CityParameter = selectedCity;
                UpdateCityPage updateCityPage = new UpdateCityPage(cities);
                
                Frame frame = FrameHolder.MangerFrame;
                frame.Navigate(updateCityPage);

            }
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCity != null)
            {
                // Call your API method to delete the city
                int result = await apiService.DeleteACity(selectedCity);

                if (result > 0)
                {
                    // If the deletion was successful, update the city list
                    List<City> cities = await apiService.SelectAllCities();

                    // Refresh the ListView
                    this.myListView.ItemsSource = null;
                    this.myListView.ItemsSource = cities;
                }
                else
                {
                    MessageBox.Show("מחיקת העיר נכשלה");
                }
            }
        }

        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedCity = item.DataContext as City;
            }
        }

        private void ListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedCity = item.DataContext as City;
            }
        }



    }
}
