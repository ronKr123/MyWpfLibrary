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
    /// Interaction logic for ReturnBooksPage.xaml
    /// </summary>
    public partial class ReturnBooksPage : Page
    {
        public ApiService api = new ApiService();

        public static LendingAndReturnsBooksList GetUsersLends { get; set; }

        public string UserSelected { get; set; }

        public string BookSelected { get; set; }

        public LendingAndReturnsBooks LendingUser { get; set; }

        public ReturnBooksPage(Model.LendingAndReturnsBooksList lendingAndReturnsBooks)
        {
            InitializeComponent();
            GetUsersLends = lendingAndReturnsBooks;
            this.txtDigitalBook.Visibility = Visibility.Collapsed;
            this.txtChooseBook.Visibility = Visibility.Collapsed;
            this.BooksNamesComboBox.Visibility = Visibility.Collapsed;
            this.txtChooseDate.Visibility = Visibility.Collapsed;
            this.datePicker.Visibility = Visibility.Collapsed;
            this.btnSend.Visibility = Visibility.Collapsed;
            this.datePicker.Text = "";
            this.allUsersNamesComboBox.SelectedItem = null;
            this.BooksNamesComboBox.SelectedItem = null;

            InitComboBoxUsersLend();

            allUsersNamesComboBox.SelectionChanged += AllUsersNamesComboBox_SelectionChanged;
            BooksNamesComboBox.SelectionChanged += BooksNamesComboBox_SelectionChanged;
        }

        private async void BooksNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookSelected = this.BooksNamesComboBox.SelectedItem as string;
            BooksList booksL = await api.SelectAllBooks();
            int idBook = 0;
            foreach(Books bookP in booksL)
            {
                if (bookP.BookName.Equals(BookSelected))
                {
                    idBook = bookP.Id;
                }
            }
            if(idBook != 0)
            {
                this.imgBook.Visibility = Visibility.Visible;
                ImageFromDataBase(idBook);
            }

            this.datePicker.Visibility = Visibility.Visible;
            this.btnSend.Visibility = Visibility.Visible;

            // get lending date of book :
            DateTime dateLending = new DateTime(2001 , 1 , 1);
            foreach(LendingAndReturnsBooks lending in GetUsersLends)
            {
                if(lending.UserCode.UserName.Equals(UserSelected) && lending.BookCode.BookName.Equals(BookSelected) && lending.DateOfReturn == new DateTime(2001, 1, 1))
                {
                    dateLending = lending.DateOfLending;
                    LendingUser = lending;
                }
            }

            if (dateLending != new DateTime(2001, 1, 1))
            {
                this.datePicker.DisplayDateStart = dateLending;
            }

        }


        private void AllUsersNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string userNameSelected = this.allUsersNamesComboBox.SelectedItem as string;
            UserSelected = userNameSelected;

            List<string> booksLendOfUser = new List<string>();
            foreach(LendingAndReturnsBooks lending in GetUsersLends)
            {
                if(lending.UserCode.UserName.Equals(userNameSelected) && lending.DateOfReturn == new DateTime(2001, 1, 1))
                {
                    booksLendOfUser.Add(lending.BookCode.BookName);
                }
            }
            this.BooksNamesComboBox.Visibility = Visibility.Visible;
            this.BooksNamesComboBox.ItemsSource = booksLendOfUser;
        }


        private void InitComboBoxUsersLend()
        {
            List<string> usersNamesLends = new List<string>();
            LendingAndReturnsBooksList lendings = GetUsersLends;
            foreach(LendingAndReturnsBooks lending in lendings)
            {
                if(lending.DateOfReturn == new DateTime(2001, 1 , 1))
                {
                    usersNamesLends.Add(lending.UserCode.UserName);
                }
            }
            this.allUsersNamesComboBox.ItemsSource = usersNamesLends;
        }



        private async void AddNewReturn(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = this.datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                LendingAndReturnsBooks returnUserBook = LendingUser;
                returnUserBook.DateOfReturn = (DateTime)selectedDate;
                int x = await api.UpdateALendingAndReturnBook(returnUserBook);
                if(x != 0)
                {
                    MessageBox.Show("החזרת הספר בוצעה בהצלחה");
                    this.txtDigitalBook.Visibility = Visibility.Collapsed;
                    this.txtChooseBook.Visibility = Visibility.Collapsed;
                    this.BooksNamesComboBox.Visibility = Visibility.Collapsed;
                    this.txtChooseDate.Visibility = Visibility.Collapsed;
                    this.datePicker.Visibility = Visibility.Collapsed;
                    this.btnSend.Visibility = Visibility.Collapsed;
                    this.imgBook.Visibility = Visibility.Collapsed;
                    this.datePicker.Text = "";
                    this.allUsersNamesComboBox.SelectedItem = null;
                    this.BooksNamesComboBox.SelectedItem = null;

                }

            }
            else
            {
                MessageBox.Show("בבקשה לבחור תאריך החזרת ספר");
                return;
            }

        }


        private async void ImageFromDataBase(int idBook)
        {
            await DoJob(idBook);
            DigitalBooksList digitalBooks = await api.SelectAllDigitalBooks();

            this.txtDigitalBook.Visibility = Visibility.Collapsed;

            foreach (DigitalBooks digitalBook in digitalBooks)
            {
                if (digitalBook.Id == idBook)
                {
                    this.txtDigitalBook.Visibility = Visibility.Visible;
                }
            }

        }

        private async Task DoJob(int idBook)
        {
            // מקבלת את המחרוזת 64 של אותו ספר ספציפי
            // הופכת את הסטרינג הארוך מאוד לביתים , ואותם ממירה לתמונה
            // שאת התמונה מוסיפים ל - source  
            string st = await api.GetBookPicByte64(idBook);
            byte[] imgStr = Convert.FromBase64String(st);
            this.imgBook.Source = ByteImageConverter.ByteToImage(imgStr);
        }


    }
}
