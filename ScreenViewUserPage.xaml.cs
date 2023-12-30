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
using Windows.Media.Audio;
using System.IO;
using ViewModel;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for ScreenViewUserPage.xaml
    /// </summary>
    public partial class ScreenViewUserPage : Page
    {
        public ScreenViewUserPage(Model.Users user)
        {
            InitializeComponent();
            this.messageHelloUser.Text = "שלום" + " " + user.FirstName + " ";
            NumLendingAndReturnsBooksOfUser(user);
            LoadRandomImage();



        }

        public async void NumLendingAndReturnsBooksOfUser(Users user)
        {
            ApiService apiService = new ApiService();
            LendingAndReturnsBooksList lendAndRetsList = await apiService.SelectAllLendingAndReturnsBooks();
            int num = 0;
            foreach(LendingAndReturnsBooks lendAndRet in lendAndRetsList)
            {
                if(lendAndRet.UserCode.Id == user.Id)
                {
                    num++;
                }
            }
            this.myLendAndRet.Text = num.ToString();
        }


        //private async void ImageFromDataBase(int idBook)
        //{
        //    await DoJob(idBook);
        //}

        //private async Task DoJob(int idBook)
        //{
        //    ApiService apiService = new ApiService();
        //    string st = await apiService.GetBookPicByte64(idBook);
        //    byte[] imgStr = Convert.FromBase64String(st);
        //    this.bookPicImgDb.Source = ByteImageConverter.ByteToImage(imgStr);

        //}

        private void ToRegisterOrLoginPage_Click(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("?אתם רוצים לצאת", "",
                                   MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response != MessageBoxResult.No)
            {
                Frame frame = FrameHolder.MainWindowFrame;
                // לעבור לעמוד הרשמה או כניסה
                // שבפריים הראשי
                // MainWindow : ב
                frame.Navigate(new LoginOrRegisterPage());
            }

        }


        private readonly string imagesFolderPath = UtilityMedia.GetPathToFolderImages();

        private void LoadRandomImage()
        {
            try
            {
                string[] imageFiles = Directory.GetFiles(imagesFolderPath);
                if (imageFiles.Length > 0)
                {
                    Random random = new Random();
                    string randomImagePath = imageFiles[random.Next(imageFiles.Length)];
                    this.bookPicImgDb.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(randomImagePath));
                }
                else
                {
                    MessageBox.Show("No images found in the specified folder.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


    }
}
