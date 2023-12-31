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
using Model;
using ApiLibraryService;
using System.Collections.ObjectModel;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for FilterBooksPage.xaml
    /// </summary>
    public partial class FilterBooksPage : Page
    {
        public ApiService api = new ApiService();

        public List<BookViewUC> bookViewUCs { get; set; }


        public static GenreList GetGenres { get; set; }

        public static WritersList GetWriters { get; set; }

        public FilterBooksPage(Model.GenreList genres, Model.WritersList writers)
        {
            InitializeComponent();
            GetGenres = genres;
            GetWriters = writers;

            List<string> genresNames = new List<string>();
            foreach(Genre genre in genres)
            {
                genresNames.Add(genre.GenreName);
            }
            this.genreComboBox.ItemsSource = genresNames;

            List<string> writersNames = new List<string>();
            foreach (Writers writer in writers)
            {
                writersNames.Add(writer.FirstName + " " + writer.LastName);
            }
            this.writerComboBox.ItemsSource = writersNames;



        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string bookTxtSearch = this.searchTextBox.Text;
            string writerFullName = this.writerComboBox.SelectedItem as string;
            string genreName = this.genreComboBox.SelectedItem as string;

            int codeWriter = GetWriterCode(writerFullName);
            int codeGenre = GetGenreCode(genreName);

            Filter filterSql = new Filter
            {
                BookNameTxt = bookTxtSearch,
                GenreBookCode = codeGenre,
                WriterBookCode = codeWriter
            };

            string sqlSentence = filterSql.BuildFilter();
            BooksList booksList = await api.SelectAllBooksWithFilter(sqlSentence);

            this.g.Children.Clear();
            this.g.ColumnDefinitions.Clear();
            this.g.RowDefinitions.Clear();

            UpdateTheGrid(booksList);

        }

        private int GetWriterCode(string writerFullName)
        {
            if (writerFullName == null)
                return 0;

            foreach (Writers w in GetWriters)
            {
                if ((w.FirstName + " " + w.LastName).Equals(writerFullName))
                {
                    return w.Id;
                }
            }
            return 0;
        }

        private int GetGenreCode(string genreName)
        {
            if (genreName == null)
                return 0;

            foreach (Genre g in GetGenres)
            {
                if (g.GenreName.Equals(genreName))
                {
                    return g.Id;
                }
            }
            return 0;
        }


        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void UpdateTheGrid(BooksList booksList)
        {
            DigitalBooksList digitalBooks = await api.SelectAllDigitalBooks();

            if (booksList.Count > 0)
            {
                int numRows = (int)Math.Ceiling((double)booksList.Count / 2);
                for (int i = 0; i < numRows; i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(350) });
                }

                for (int i = 0; i < 2; i++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(350) });
                }
            }

            int j = 0, k = 0;
            foreach (Books book in booksList)
            {
                BookViewUC.BookParameter = book;
                BookViewUC.DigitalBooksParameter = digitalBooks;

                BookViewUC bookViewUC = new BookViewUC();
                bookViewUC.Margin = new Thickness(7);

                Grid.SetRow(bookViewUC, j);
                Grid.SetColumn(bookViewUC, k);
                g.Children.Add(bookViewUC);

                j++;

                if (j == booksList.Count / 2 + 1)
                {
                    k++;
                    j = 0;
                }
            }

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Adjust the vertical offset of the ScrollViewer based on the mouse wheel delta
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 4.0);

            // Mark the event as handled to prevent further processing by other elements
            e.Handled = true;
        }


    }
}
