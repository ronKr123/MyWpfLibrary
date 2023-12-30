using ApiLibraryService;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PageListBooks.xaml
    /// </summary>
    public partial class PageListBooks : Page
    {
        public static Users Userr1 { get; set; }


        public static BookViewUC BookViewUC1;


        public PageListBooks(Model.BooksList books)
        {
            InitializeComponent();

            Show(books);


            
        }

        private void RefreshPageAfterUpdates()
        {
            InitializeComponent();
        }


        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Adjust the vertical offset of the ScrollViewer based on the mouse wheel delta
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 4.0);

            // Mark the event as handled to prevent further processing by other elements
            e.Handled = true;
        }


        public async Task Show(BooksList booksList)
        {
            //Users user = Userr1;

            ApiService apiService = new ApiService();

            DigitalBooksList digitalBooks = await apiService.SelectAllDigitalBooks();


            RowDefinition gridRow;

            for (int i = 0; i < (booksList.Count / 2 ) + 1 ; i++)
            {
                gridRow = new RowDefinition();
                gridRow.Height = new GridLength(350);
                g.RowDefinitions.Add(gridRow);
            }

            ColumnDefinition gridCol;
            for (int i = 0; i < 2; i++)
            {
                gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(350);
                g.ColumnDefinitions.Add(gridCol);
            }

            int k = 0, j = 0;

            foreach (Books book in booksList)
            {
                Users user = Userr1;

                BookViewUC.BookParameter = book;
                BookViewUC.DigitalBooksParameter = digitalBooks;

                BookViewUC bUC = new BookViewUC();

                bUC.Margin = new Thickness(7);
                Grid.SetRow(bUC, j);
                Grid.SetColumn(bUC, k);

                j++;

                if (j == booksList.Count / 2)
                {
                    k++;
                    j = 0;
                }
                g.Children.Add(bUC);
            }
        }


    }
}
