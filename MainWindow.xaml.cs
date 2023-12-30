using ApiLibraryService;
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
using ViewModel;
using Model;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        ApiService api = new ApiService();

        public MainWindow()
        {
            InitializeComponent();
            FrameHolder.MainWindowFrame = this.frame;

            Closing += MainWindow_Closing;
        }


        private async void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
           api.CloseDB();
        }


    }

}
