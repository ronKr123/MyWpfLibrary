using ApiLibraryService;
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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for PageDigitalBooks.xaml
    /// </summary>
    public partial class PageDigitalBooks : Page
    {
        ApiService apiService = new ApiService();

        public PageDigitalBooks(Model.DigitalBooksList digitalBooksList)
        {
            InitializeComponent();

            Show(digitalBooksList);

        }


        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Adjust the vertical offset of the ScrollViewer based on the mouse wheel delta
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 4.0);

            // Mark the event as handled to prevent further processing by other elements
            e.Handled = true;
        }


        public async Task Show(DigitalBooksList digitalBooksList)
        {


            RowDefinition gridRow;
            for (int i = 0; i < (digitalBooksList.Count / 2)+1 ; i++)
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

            foreach (DigitalBooks digiBook in digitalBooksList)
            {
                AudioBookUC aUC = new AudioBookUC(digiBook);
                aUC.Margin = new Thickness(7);
                //Grid.SetRow(aUC, j);
                //Grid.SetColumn(aUC, k);

                if(g.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == j && Grid.GetColumn(e) == k) == null)
                {
                    Grid.SetRow(aUC, j);
                    Grid.SetColumn(aUC, k);
                    g.Children.Add(aUC);
                }

                j++;

                if (j == digitalBooksList.Count / 2 + 1)
                {
                    k++;
                    j = 0;
                }
                //g.Children.Add(aUC);
            }
        }


    }
}
