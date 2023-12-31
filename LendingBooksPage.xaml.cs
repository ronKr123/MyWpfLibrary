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
    /// Interaction logic for LendingBooksPage.xaml
    /// </summary>
    public partial class LendingBooksPage : Page
    {
        public static ApiService api = new ApiService();
        public static BooksList GetBooks { get; set; }
        public static UsersList GetUsers { get; set; }

        public LendingBooksPage(Model.BooksList booksList, Model.UsersList users)
        {
            InitializeComponent();
            GetBooks = booksList;
            GetUsers = users;
            this.imgBook.Visibility = Visibility.Collapsed;

            InitComboBoxes();

            BooksNamesComboBox.SelectionChanged += BooksNamesComboBox_SelectionChanged;
        }


        private void BooksNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string bookNameSelected = this.BooksNamesComboBox.SelectedItem as string;

            this.txtDigitalBook.Visibility = Visibility.Collapsed;

            BooksList books = GetBooks;
            int bookId = 0;
            foreach(Books book in books)
            {
                if (book.BookName.Equals(bookNameSelected))
                {
                    bookId = book.Id;
                }
            }

            if (bookId != 0)
            {
                this.imgBook.Visibility = Visibility.Visible;
                ImageFromDataBase(bookId);
            }

        }

        private async void ImageFromDataBase(int idBook)
        {
            await DoJob(idBook);
            DigitalBooksList digitalBooks = await api.SelectAllDigitalBooks();
            foreach(DigitalBooks digitalBook in digitalBooks)
            {
                if(digitalBook.Id == idBook)
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


        private void InitComboBoxes()
        {
            List<string> usersNames = new List<string>();
            UsersList users = GetUsers;
            foreach(Users user in users)
            {
                usersNames.Add(user.UserName);
            }
            this.allUsersNamesComboBox.ItemsSource = usersNames;

            List<string> booksNames = new List<string>();
            BooksList books = GetBooks;
            foreach(Books book in books)
            {
                booksNames.Add(book.BookName);
            }
            this.BooksNamesComboBox.ItemsSource = booksNames;

        }


        private async void AddNewLending(object sender, RoutedEventArgs e)
        {
            LendingAndReturnsBooks lending = new LendingAndReturnsBooks();

            string userNameSelected = this.allUsersNamesComboBox.SelectedItem as string;
            string bookNameSelected = this.BooksNamesComboBox.SelectedItem as string;
            if (userNameSelected != null)
            {
                foreach (Users user in GetUsers)
                {
                    if (user.UserName.Equals(userNameSelected))
                    {
                        lending.UserCode = user;
                    }
                }
            }
            else
            {
                MessageBox.Show("בבקשה לבחור משתמש");
                return;
            }

            if (bookNameSelected != null)
            {
                foreach (Books book in GetBooks)
                {
                    if (book.BookName.Equals(bookNameSelected))
                    {
                        lending.BookCode = book;
                    }
                }
            }
            else
            {
                MessageBox.Show("בבקשה לבחור ספר");
                return;
            }

            DateTime? selectedDate = this.datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                lending.DateOfLending = (DateTime)selectedDate;
            }
            else
            {
                MessageBox.Show("בבקשה לבחור תאריך השאלת ספר");
                return;
            }

            lending.DateOfReturn = new DateTime(2001, 1, 1);

            int x = await api.InsertALendingAndReturnBook(lending);
            if(x != 0)
            {
                MessageBox.Show("ההשאלה בוצעה בהצלחה");
                this.allUsersNamesComboBox.SelectedItem = null;
                this.BooksNamesComboBox.SelectedItem = null;
                this.datePicker.Text = "";
                this.imgBook.Visibility = Visibility.Collapsed;
                this.txtDigitalBook.Visibility = Visibility.Collapsed;
            }
        }


    }
}
