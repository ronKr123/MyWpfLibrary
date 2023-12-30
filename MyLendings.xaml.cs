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
using ApiLibraryService;
using Model;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for MyLendings.xaml
    /// </summary>
    public partial class MyLendings : Page
    {
        ApiService apiService = new ApiService();

        public MyLendings(Model.LendingAndReturnsBooksList myLendingsList)
        {
            InitializeComponent();

            if (myLendingsList != null)
            {
                Show(myLendingsList);
            }
            else
            {
                NoLending();
            }

        }

        public void NoLending()
        {
            Grid grid = (Grid)this.FindName("g");

            if (grid != null)
            {
                // Create a TextBlock
                TextBlock bigTextBlock = new TextBlock();
                bigTextBlock.Text = "אין השאלות";
                bigTextBlock.FontSize = 36; // Set the font size to your desired value
                bigTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                bigTextBlock.VerticalAlignment = VerticalAlignment.Center;

                // Add the TextBlock to the Grid
                grid.Children.Add(bigTextBlock);
            }
        }

        //public async void GetMyLendingsList(Users user)
        //{
        //    LendingAndReturnsBooksList lendingAndReturnsBooksList = await apiService.SelectAllLendingAndReturnsBooks();

        //    LendingAndReturnsBooksList myLendingList = new LendingAndReturnsBooksList();

        //    foreach(LendingAndReturnsBooks lendAndRet in lendingAndReturnsBooksList)
        //    {
        //        if(lendAndRet.UserCode.Id == user.Id)
        //        {
        //            myLendingList.Add(lendAndRet);
        //        }
        //    }

        //    MyLendingsList = myLendingList;
        //}


        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Adjust the vertical offset of the ScrollViewer based on the mouse wheel delta
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 4.0);

            // Mark the event as handled to prevent further processing by other elements
            e.Handled = true;
        }


        public async Task Show(LendingAndReturnsBooksList myLendings)
        {
            RowDefinition gridRow;
            for (int i = 0; i < (myLendings.Count + 1) / 2; i++)
            {
                gridRow = new RowDefinition();
                gridRow.Height = new GridLength(300);
                g.RowDefinitions.Add(gridRow);
            }

            ColumnDefinition gridCol;
            for (int i = 0; i < 2; i++)
            {
                gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(300);
                g.ColumnDefinitions.Add(gridCol);
            }

            int k = 0, j = 0;

            foreach (LendingAndReturnsBooks lendAndRet in myLendings)
            {
                LendAndRetUC lendAndRetUC = new LendAndRetUC(lendAndRet);

                lendAndRetUC.Margin = new Thickness(5);
                Grid.SetRow(lendAndRetUC, j);
                Grid.SetColumn(lendAndRetUC, k);

                j++;
                if (j == 2)
                {
                    k++;
                    j = 0;
                }
                g.Children.Add(lendAndRetUC);
            }
        }



    }
}
