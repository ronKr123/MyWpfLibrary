using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ViewModel;
using ApiLibraryService;
using System.Windows;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using NAudio.Wave;

namespace WpfLibrary
{
    public class AddNewDigitalBookPage: AddingNewBookPage
    {
        ApiService api1 = new ApiService();




        public AddNewDigitalBookPage() : base()
        {
            this.txtAudioFile.Visibility = System.Windows.Visibility.Visible;
            this.fileAudioPathTxt.Visibility = System.Windows.Visibility.Visible;
            this.uploadAudioFile.Visibility = System.Windows.Visibility.Visible;


        }

        //public async void ShowGenres()
        //{
        //    GenreList genres = await api.SelectAllGeneres();
        //    List<string> genersNames = new List<string>();
        //    if (genres != null)
        //    {
        //        foreach (Genre g in genres)
        //        {
        //            genersNames.Add(g.GenreName);
        //        }
        //        this.genreBookComboBox.ItemsSource = genersNames;
        //    }
        //}


        //public async void ShowWriters()
        //{
        //    WritersList writers = await api.SelectAllWriters();
        //    List<string> writersNames = new List<string>();
        //    if (writers != null)
        //    {
        //        foreach (Writers w in writers)
        //        {
        //            writersNames.Add(w.FirstName + " " + w.LastName);
        //        }
        //        this.writersNamesComboBox.ItemsSource = writersNames;
        //    }
        //}


        //private void uploadImage_Click(object sender, RoutedEventArgs e)
        //{
        //    // Create OpenFileDialog

        //    Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

        //    dialog.Filter = "Images files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        //    Nullable<bool> result = dialog.ShowDialog();

        //    if (result == true)
        //    {
        //        // Open Document
        //        string fileName = dialog.FileName;
        //        this.fileImagePathTxt.Text = System.IO.Path.GetFileName(fileName);

        //        ImageFileName = fileName;
        //        MessageBox.Show("התמונה נבחרה בהצלחה");
        //    }
        //}


        public async override void AddingNewBook_Click(object sender, RoutedEventArgs e)
        {
            DigitalBooks digitalBook = new DigitalBooks();
            digitalBook.BookName = this.bookNameTxtBox.Text;

            string genreNameSelected = this.genreBookComboBox.SelectedItem as string;
            GenreList genres = await api1.SelectAllGeneres();
            foreach (Genre g in genres)
            {
                if (g.GenreName.Equals(genreNameSelected))
                {
                    digitalBook.GenreCode = g;
                }
            }

            string writerFullNameSelected = this.writersNamesComboBox.SelectedItem as string;
            WritersList writers = await api1.SelectAllWriters();
            foreach (Writers w in writers)
            {
                string writerFullName = w.FirstName + " " + w.LastName;
                if (writerFullName.Equals(writerFullNameSelected))
                {
                    digitalBook.WriterCode = w;
                }
            }

            DateTime? selectedDate = this.datePicker.SelectedDate;
            if (selectedDate.HasValue)
            {
                digitalBook.DateOfPublishBook = (DateTime)selectedDate;
            }
            digitalBook.BookPic = "xx";

            string imageFileName = ImageFileName;
            string audioFileName = AudioFileName;

            if (imageFileName != null && audioFileName != null)
            {
                digitalBook.PictureBook = System.IO.Path.GetFileName(ImageFileName);

                byte[] imgArray = System.IO.File.ReadAllBytes(ImageFileName);

               UtilityMedia.SaveImages(imgArray, imageFileName);

                digitalBook.BookAudioFile = System.IO.Path.GetFileName(audioFileName);

                digitalBook.Duration = GetAudioFileLengthInMinutes(audioFileName);

                UtilityMedia.SaveAudioFiles(audioFileName);

                int x = await api1.InsertADigitalBook(digitalBook);
                if (x != 0)
                {
                    MessageBox.Show("הספר הוסף בהצלחה");
                    this.bookNameTxtBox.Text = "";
                    this.genreBookComboBox.SelectedItem = null;
                    this.writersNamesComboBox.SelectedItem = null;
                    this.datePicker.Text = "";
                    this.fileImagePathTxt.Text = "";
                    this.fileAudioPathTxt.Text = "";

                }
                else
                {
                    MessageBox.Show("יש בעיה עם הוספת הספר");
                }

            }
        }

        public int GetAudioFileLengthInMinutes(string filePath)
        {
            try
            {
                using (var audioFile = new AudioFileReader(filePath))
                {
                    // Get the total time in seconds
                    double totalTimeInSeconds = audioFile.TotalTime.TotalSeconds;

                    // Convert the total time to minutes and round it to the nearest integer
                    int totalTimeInMinutes = (int)Math.Round(totalTimeInSeconds / 60.0);

                    return totalTimeInMinutes;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., file not found or invalid audio file format
                Console.WriteLine($"Error: {ex.Message}");
                return -1; // Return a default or error value
            }
        }


        public override void UploadAudioFile(object sender, RoutedEventArgs e)
           {
             OpenFileDialog openFileDialog = new OpenFileDialog
             {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Title = "Select an MP3 File"
             };

            if (openFileDialog.ShowDialog() == true)
            {

                // Open Document
                string fileName = openFileDialog.FileName;

                this.fileAudioPathTxt.Text = System.IO.Path.GetFileName(fileName);

                AudioFileName = fileName;
                MessageBox.Show("קובץ השמע נבחר בהצלחה");
            }
        }


    }
}
