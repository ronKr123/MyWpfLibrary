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
    /// Interaction logic for ScreenViewManger.xaml
    /// </summary>
    public partial class ScreenViewManger : Page
    {
        ApiService api = new ApiService();

        public static int CountUsers { get; set; }
        public static int CountBooks { get; set; }
        public static int CountLendsAndRets { get; set; }
        public static int CountWriters { get; set; }

        public ScreenViewManger()
        {
            InitializeComponent();
            GetNumUseres();
            GetNumBooks();
            GetNumLendsAndRets();
            GetNumWriters();

            this.countUseresTxtBlock.Text = $"משתמשים {CountUsers}";
            this.countBooksTxtBlock.Text = $"ספרים {CountBooks}";
            this.countLendAndRetsTxtBlock.Text = $"השאלות והחזרות {CountLendsAndRets}";
            this.countWritersTxtBlock.Text = $"סופרים {CountWriters}";


        }


        public async Task GetNumUseres()
        {
            UsersList users = await api.SelectAllUsers();
            if (users == null)
            {
                users = await api.SelectAllUsers();
            }
            int numUseres = users.Count;
            CountUsers = numUseres;
        }

        public async Task GetNumBooks()
        {
            BooksList books = await api.SelectAllBooks();
            if(books == null)
            {
                books = await api.SelectAllBooks();
            }
            int numBooks = books.Count;
            CountBooks = numBooks;
        }

        public async Task GetNumLendsAndRets()
        {
            LendingAndReturnsBooksList lendingAndReturns = await api.SelectAllLendingAndReturnsBooks();
            if(lendingAndReturns == null)
            {
                lendingAndReturns = await api.SelectAllLendingAndReturnsBooks();
            }
            int numLendsAndRets = lendingAndReturns.Count;
            CountLendsAndRets = numLendsAndRets;
        }

        public async Task GetNumWriters()
        {
            WritersList writers = await api.SelectAllWriters();
            if(writers == null)
            {
                writers = await api.SelectAllWriters();
            }
            int numWriters = writers.Count;
            CountWriters = numWriters;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
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

    }
}
