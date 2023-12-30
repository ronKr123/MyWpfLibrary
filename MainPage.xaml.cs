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
using Windows.UI.Xaml.Controls;
using ViewModel;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page
    {
        public static System.Windows.Controls.Frame Frame;
        public static Users Userr { get; set; }

        ApiService api = new ApiService();


        public MainPage()
        {

            InitializeComponent();

            Users userPlay = Userr;

            FrameHolder.MainFrameUser = this.frame;

            Frame = this.frame;
            this.frame.Navigate(new ScreenViewUserPage(userPlay));

           


        }

        private void BookViewUC_RequestNavigateToAnotherPage(object sender, EventArgs e)
        {
            //// Handle the event by navigating to AnotherPage
            //frame.Navigate(new Uri("Pages/BookPage.xaml", UriKind.Relative));

        }

        private async void HamburgerMenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Users userPlay = Userr;
            if (userPlay != null)
            {
                this.frame.Navigate(new ScreenViewUserPage(userPlay));
            }
            else
            {
                MessageBox.Show("שגיאה! לא מוצא משתמש");
            }

        }

        private async Task ToBooksPageAsync(object sender, MouseButtonEventArgs e)
        {
            BooksList books = await api.SelectAllBooks();
            Users userPlay = Userr;
            PageListBooks.Userr1 = userPlay;

            this.frame.Navigate(new PageListBooks(books));
        }

        private async void ToBooksPage(object sender, MouseButtonEventArgs e)
        {
            BooksList books = await api.SelectAllBooks();
            Users userPlay = Userr;
            PageListBooks.Userr1 = userPlay;
            this.frame.Navigate(new PageListBooks(books));
        }

        private async void UpdateUserDetails(object sender, MouseButtonEventArgs e)
        {
            Users userPlay = Userr;

            CityList cities = await api.SelectAllCities();
            if (userPlay != null)
            {
                UpdateUserPage.Userr = userPlay;
                this.frame.Navigate(new UpdateUserPage(cities));
            }
            else
            {
                MessageBox.Show("שגיאה");
            }

        }

        //private void addNewBookClick(object sender, MouseButtonEventArgs e)
        //{
        //    this.frame.Navigate(new AddingNewBookPage());
        //}

        private async void digitalBooksPage(object sender, MouseButtonEventArgs e)
        {
            DigitalBooksList digitalBooksList = await api.SelectAllDigitalBooks();
            this.frame.Navigate(new PageDigitalBooks(digitalBooksList));
        }


        //private void findBook(object sender, MouseButtonEventArgs e)
        //{
        //    this.frame.Navigate(new AddNewDigitalBookPage());

        //}

        private async void ToMyLendingsPage(object sender, MouseButtonEventArgs e)
        {
            Users user = Userr;
            LendingAndReturnsBooksList myLendingList = await GetMyLendingsList(user);
            this.frame.Navigate(new MyLendings(myLendingList));
        }


        private async Task<LendingAndReturnsBooksList> GetMyLendingsList(Users user)
        {
            LendingAndReturnsBooksList lendingAndReturnsBooksList = await api.SelectAllLendingAndReturnsBooks();

            LendingAndReturnsBooksList myLendingList = new LendingAndReturnsBooksList();

            foreach (LendingAndReturnsBooks lendAndRet in lendingAndReturnsBooksList)
            {
                if (lendAndRet.UserCode.Id == user.Id)
                {
                    myLendingList.Add(lendAndRet);
                }
            }

            return myLendingList;
        }


        private async void ToSearchBookPage(object sender, MouseButtonEventArgs e)
        {
            GenreList genres = await api.SelectAllGeneres();

            WritersList writers = await api.SelectAllWriters();

            this.frame.Navigate(new FilterBooksPage(genres, writers));

        }

    }
}
