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
    /// Interaction logic for WritesList.xaml
    /// </summary>
    public partial class WritesList : Page
    {
        ApiService apiService = new ApiService();

        private Writers selectedWriter;

        private async void LoadWritersAsync()
        {
            List<Writers> writers = await GetWrites();
            //DataContext = userList;
            this.myListView.ItemsSource = writers;
        }

        private async Task<List<Writers>> GetWrites()
        {
            WritersList writers = await apiService.SelectAllWriters();
            return writers.ToList();
        }


        public WritesList()
        {
            InitializeComponent();
            LoadWritersAsync();
        }

        private void userListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedWriter = item.DataContext as Writers;
            }
        }

        private void userListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedWriter = item.DataContext as Writers;
            }
        }

        private void myListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myListView.SelectedItem != null)
            {
                selectedWriter = myListView.SelectedItem as Writers;
            }
            else
            {
                selectedWriter = null;
            }
        }

        private async void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedWriter != null)
            {
                GenreList genres = await apiService.SelectAllGeneres();
                UpdateWriterPage.WriterParameter = selectedWriter;
                UpdateWriterPage updateWriterPage = new UpdateWriterPage(genres);
                Frame frame = FrameHolder.MangerFrame;
                frame.Navigate(updateWriterPage);
            }
        }


        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedWriter != null)
            {
                // Call your API method to delete the city
                int result = await apiService.DeleteAWriter(selectedWriter);

                if (result > 0)
                {
                    // If the deletion was successful, update the city list
                    List<Writers> writers = await apiService.SelectAllWriters();

                    // Refresh the ListView
                    this.myListView.ItemsSource = null;
                    this.myListView.ItemsSource = writers;
                }
                else
                {
                    MessageBox.Show("מחיקת הסופר נכשלה");
                }
            }
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            GenreList genres = await apiService.SelectAllGeneres();

            Frame frame = FrameHolder.MangerFrame;
            frame.Navigate(new InsertNewWriter(genres));
        }


    }
}
