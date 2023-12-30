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
    /// Interaction logic for GenresListPage.xaml
    /// </summary>
    public partial class GenresListPage : Page
    {

        ApiService apiService = new ApiService();

        private Genre selectedGenre;

        public GenresListPage()
        {
            InitializeComponent();
            LoadGenresAsync();

        }


        private async void LoadGenresAsync()
        {
            List<Genre> genres = await GetGenres();
            //DataContext = userList;
            this.myListView.ItemsSource = genres;
        }

        private async Task<List<Genre>> GetGenres()
        {
            GenreList genres = await apiService.SelectAllGeneres();
            return genres.ToList();
        }


        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedGenre = item.DataContext as Genre;
            }
        }

        private void ListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedGenre = item.DataContext as Genre;
            }
        }

        private async void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGenre != null)
            {
                GenreList genres = await apiService.SelectAllGeneres();
                UpdateGenrePage.GenreParameter = selectedGenre;
                UpdateGenrePage updateGenrePage = new UpdateGenrePage(genres);
                Frame frame = FrameHolder.MangerFrame;
                frame.Navigate(updateGenrePage);

            }
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGenre != null)
            {
                // Call your API method to delete the genre
                int result = await apiService.DeleteAGenre(selectedGenre);

                if (result > 0)
                {
                    // If the deletion was successful, update the Genres list
                    List<Genre> genres = await apiService.SelectAllGeneres();

                    // Refresh the ListView
                    this.myListView.ItemsSource = null;
                    this.myListView.ItemsSource = genres;
                }
                else
                {
                    MessageBox.Show("מחיקת הז'אנר נכשלה");
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            GenreList genres = await apiService.SelectAllGeneres();

            Frame frame = FrameHolder.MangerFrame;
            frame.Navigate(new AddGenrePage(genres));

        }


        private void myListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myListView.SelectedItem != null)
            {
                selectedGenre = myListView.SelectedItem as Genre;
            }
            else
            {
                selectedGenre = null;
            }
        }


    }
}
