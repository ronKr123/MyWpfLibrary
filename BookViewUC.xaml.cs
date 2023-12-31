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
using ViewModel;
using Windows.System;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for BookViewUC.xaml
    /// </summary>
    public partial class BookViewUC : UserControl
    {
        public Users user1;

        ApiService api = new ApiService();

        public static Books BookParameter { get; set; }
        public static DigitalBooksList DigitalBooksParameter { get; set; }


        


        public BookViewUC( )
        {
            InitializeComponent();

            DigitalBooksList parameterValueDigitalBooksList = DigitalBooksParameter;

            Books parameterValueBook = BookParameter;


            
            if (parameterValueBook != null)
            {
                this.myBookName.Visibility = Visibility.Visible;
                this.picBook.Visibility = Visibility.Visible;
                this.digitalBookType.Visibility = Visibility.Visible;
                this.btnDeatailsOfbook.Visibility = Visibility.Visible;

                this.myBookName.Text = parameterValueBook.BookName;
                int idBook = parameterValueBook.Id;
                ImageFromDataBase(idBook);
                // הוספת תמונת הספר ממסד הנתונים

                this.digitalBookType.Visibility = Visibility.Collapsed;
                ChangeTypeBook(parameterValueBook, parameterValueDigitalBooksList);
                //אם הספר הוא ספר דיגיטלי יופיע המילים "ספר דיגיטלי תחתיו" מתחת לשם הספר
                // אם הוא ספר רגיל, הכיתוב לא יופיע כלל
                // בפעולה ChangeTypeBook

            }


        }
        
        private async void ImageFromDataBase(int idBook)
        {
            await DoJob(idBook);
           
        }

        private async Task DoJob(int idBook)
        {
            // מקבלת את המחרוזת 64 של אותו ספר ספציפי
            // הופכת את הסטרינג הארוך מאוד לביתים , ואותם ממירה לתמונה
            // שאת התמונה מוסיפים ל - source  

            ApiService apiService = new ApiService();
            string st = await apiService.GetBookPicByte64(idBook);
            byte[] imgStr = Convert.FromBase64String(st);
            this.picBook.Source = ByteImageConverter.ByteToImage(imgStr);
        }

        public void ChangeTypeBook(Books book, DigitalBooksList digitalBooks)
        { 
            int id = book.Id;
            foreach(DigitalBooks digiBook in digitalBooks)
            {
                if (digiBook.Id == id)
                {
                    this.digitalBookType.Visibility = Visibility.Visible;
                }
            }
        }

        public void AddingToFavBooksList(object sender, MouseEventArgs e)
        {
           
            //if (user1.FavoriteBooksList.Contains(book1))
            //{

            //    user1.FavoriteBooksList.Remove(book1);
            //    MessageBox.Show("הספר הוסר מרשימת הספרים האהובים");

            //}
            //else
            //{ 
            //     user1.FavoriteBooksList.Add(book1);
            //    MessageBox.Show("הספר הוסר מרשימת הספרים האהובים");

            //}
        }

       
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = FrameHolder.MainFrameUser;
            Frame frameManger = FrameHolder.MangerFrame;

            //עבירת הפריים למסך המציג את פרטי הספר

            Books parameterValueBook1 = BookParameter;

            BooksList bList = await api.SelectAllBooks();

            BookPage.PublicVariableBook = bList.Find(item => item.BookName.Equals(this.myBookName.Text));

            if (frame != null)
            {
                frame.Navigate(new BookPage());
            }

            if(frameManger != null)
            {
                frameManger.Navigate(new BookPage());
            }

        }

        



    }
}
