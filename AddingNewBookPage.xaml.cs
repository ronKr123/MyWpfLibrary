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
using ViewModel;
using ApiLibraryService;
using Windows.System;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for AddingNewBookPage.xaml
    /// </summary>
    public partial class AddingNewBookPage : Page
    {
        ApiService api = new ApiService();

        public static string ImageFileName { get; set; }
        public static string AudioFileName { get; set; }


        public AddingNewBookPage()
        {
            InitializeComponent();
            ShowGenres();
            ShowWriters();


        }

        public async void ShowGenres()
        {
            GenreList genres = await api.SelectAllGeneres();
            List<string> genersNames = new List<string>();
            if (genres != null)
            {
                foreach (Genre g in genres)
                {
                    genersNames.Add(g.GenreName);
                }
                this.genreBookComboBox.ItemsSource = genersNames;
            }
        }


        public async void ShowWriters()
        {
            WritersList writers = await api.SelectAllWriters();
            List<string> writersNames = new List<string>();
            if(writers != null)
            {
                foreach(Writers w in writers)
                {
                    writersNames.Add(w.FirstName + " " + w.LastName);
                }
                this.writersNamesComboBox.ItemsSource = writersNames;
            }
        }


        private void uploadImage_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "Images files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            Nullable<bool> result = dialog.ShowDialog();

            if(result == true)
            {
                // Open Document
                string fileName = dialog.FileName;
                this.fileImagePathTxt.Text = System.IO.Path.GetFileName(fileName);

                ImageFileName = fileName;
                MessageBox.Show("התמונה נבחרה בהצלחה");
            }
        }


        public virtual async void AddingNewBook_Click(object sender, RoutedEventArgs e)
        {
            Books book = new Books();
            book.BookName = this.bookNameTxtBox.Text;

            string genreNameSelected = this.genreBookComboBox.SelectedItem as string;
            GenreList genres = await api.SelectAllGeneres();
            foreach(Genre g in genres)
            {
                if (g.GenreName.Equals(genreNameSelected))
                {
                    book.GenreCode = g;
                }
            }

            string writerFullNameSelected = this.writersNamesComboBox.SelectedItem as string;
            WritersList writers = await api.SelectAllWriters();
            foreach(Writers w in writers)
            {
                string writerFullName = w.FirstName + " " + w.LastName;
                if (writerFullName.Equals(writerFullNameSelected))
                {
                    book.WriterCode = w;
                }
            }

            DateTime? selectedDate = this.datePicker.SelectedDate;
            if (selectedDate.HasValue)
            {
                book.DateOfPublishBook = (DateTime)selectedDate;
            }
            book.BookPic = "xx";

            string st = ImageFileName;
            if (st != null)
            {
                book.PictureBook = System.IO.Path.GetFileName(ImageFileName);
                
                byte[] imgArray = System.IO.File.ReadAllBytes(ImageFileName);

                UtilityMedia.SaveImages(imgArray, st);

                int x = await api.InsertABook(book);
                if (x != 0)
                {
                    MessageBox.Show("הספר הוסף בהצלחה");
                    this.bookNameTxtBox.Text = "";
                    this.genreBookComboBox.SelectedItem = null;
                    this.writersNamesComboBox.SelectedItem = null;
                    this.datePicker.Text = "";
                    this.fileImagePathTxt.Text = "";
                }
                else
                {
                    MessageBox.Show("יש בעיה עם ההלאת התמונה");
                }

            }
           
        }


        public virtual void UploadAudioFile(object sender, RoutedEventArgs e)
        {
            // הפעולה המלאה מופיעה בדף היורש את דף זה
            MessageBox.Show("נשלח");

        }


    }
}
