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
using NAudio.Wave;
using System.IO;
using ApiLibraryService;

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for AudioBookPage.xaml
    /// </summary>
    public partial class AudioBookPage : Page
    {
        ApiService apiService = new ApiService();

        // קובץ השמע הוא למעשה אירוע גל
        // waveOutEvent

        public static WaveOutEvent waveOut;

        // יצירת נגן

        public static IWavePlayer audioPlayer;

        // יצירת קורא קובץ השמע

        public static Mp3FileReader audioFileReader;

        // יצירת טיימר - עבור ה-proccesBar

        public System.Windows.Threading.DispatcherTimer timer;

        public static DigitalBooks DigitalBook { get; set; }

        public static string AudioFileName { get; set; }
                    
        // מפני שמחרוזת הקישור לקטבץ האודיו הוא קבוע , אני יוצר אותו פעם אחת ושומר אותו במשתנה הסטטי בזיכרון
        // וכך לא אצטרך לבנות את אותה המחרוזת לקובץ האודיו בכל פעם שאלחץ על כפתור התחל ניגון

        public static string FullPathToAudioFile { get; set; }

        // Flag to track whether the Play button was pressed
        private bool playButtonPressed = false;

        public AudioBookPage(Model.DigitalBooks digitalBook, string audioFileName)
        {
            InitializeComponent();
            DigitalBook = digitalBook;
            AudioFileName = audioFileName;
            ImageFromDataBase(digitalBook.Id);
            this.bookName.Text = digitalBook.BookName;
            FullPathToAudioFile = GetFullPathToAudioFile();


            waveOut = new WaveOutEvent();
            // Set the initial volume (1.0 is full volume)

            waveOut.Volume = 0.5f;

            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;

            // Set up the timer for progress bar updates
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1); // Update every 100 milliseconds
            timer.Tick += Timer_Tick;


            // Set initial button states
            this.StopButton.IsEnabled = false;
            this.ContinueButton.IsEnabled = false;
            this.forwardButton.IsEnabled = false;
            this.backwardButton.IsEnabled = false;
            this.PlayButton.IsEnabled = true;

            // Disable volume buttons initially
            this.VolumeUpButton.IsEnabled = false;
            this.VolumeDownButton.IsEnabled = false;

            // Attach the Unloaded event handler
            Unloaded += AudioPageUnloaded;

        }

        private void AudioPageUnloaded(object sender, RoutedEventArgs e)
        {
            // Ensure that audio playback is stopped when the page is unloaded
            waveOut?.Stop();
            StopProgressBarUpdater();
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

            string st = await apiService.GetBookPicByte64(idBook);
            byte[] imgStr = Convert.FromBase64String(st);
            this.digiBookImage.Source = ByteImageConverter.ByteToImage(imgStr);
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Check if the audio is at or near the end
            if (audioFileReader != null && (audioFileReader.TotalTime - audioFileReader.CurrentTime).TotalSeconds > 1)
            {
                // Update the progress bar value only if the audio is not near the end
                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = audioFileReader.CurrentTime.TotalSeconds;
                });
            }

            // Check if the audio is at or near the end
            if (audioFileReader != null && (audioFileReader.TotalTime - audioFileReader.CurrentTime).TotalSeconds <= 1)
            {
                // Disable the Forward button when the audio is at or near the end
                Dispatcher.Invoke(() =>
                {
                    this.forwardButton.IsEnabled = false;
                });
            }
            else
            {
                // Enable the Forward button when the audio is not at the end
                Dispatcher.Invoke(() =>
                {
                    this.forwardButton.IsEnabled = true;
                });
            }
        }

        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            StopProgressBarUpdater();

            // Check if playback stopped because the audio finished
            // Check if playback stopped because the audio finished
            if (e.Exception == null && waveOut.PlaybackState == PlaybackState.Stopped && audioFileReader != null && audioFileReader.CurrentTime >= audioFileReader.TotalTime)
            {
                // Reset UI elements when audio playback finishes
                Dispatcher.Invoke(() =>
                {
                    PlayButton.IsEnabled = true;
                    StopButton.IsEnabled = false;
                    ContinueButton.IsEnabled = false;
                    backwardButton.IsEnabled = false;
                    forwardButton.IsEnabled = false; // Assuming ForwardButton is disabled at the end of the audio
                    progressBar.Value = 0; // Reset the progress bar to the start
                    this.VolumeUpButton.IsEnabled = false;
                    this.VolumeDownButton.IsEnabled = false;
                });
            }


        }

        private void StopProgressBarUpdater()
        {
            // Stop the progress bar updater timer
            timer.Stop();
        }

        private void StartProgressBarUpdater()
        {
            // Start the timer to update the progress bar
            timer.Start();
        }


        private string GetFullPathToAudioFile()
        {
            string fileName = AudioFileName;
            string path = UtilityMedia.GetPathToAudioBookFile(fileName);
            return path;
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            string path = FullPathToAudioFile;

            if (File.Exists(path))
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                    StopProgressBarUpdater();
                }


                // Set the initial volume (1.0 is full volume)

                waveOut.Volume = 1.0f; 


                // Create a new instance of Mp3FileReader with the correct path
                audioFileReader = new Mp3FileReader(path);

                // Set the current time to zero before playing
                audioFileReader.CurrentTime = TimeSpan.Zero;

                // Start playing the audio
                waveOut.Init(audioFileReader);
                waveOut.Play();
                progressBar.Maximum = audioFileReader.TotalTime.TotalSeconds;
                StartProgressBarUpdater();

                // Enable Stop and Continue buttons, and disable Play button
                this.StopButton.IsEnabled = true;
                this.ContinueButton.IsEnabled = false;
                this.forwardButton.IsEnabled = true;
                this.backwardButton.IsEnabled = true;
                this.PlayButton.IsEnabled = false;

                // Enable volume buttons when Play button is pressed
                playButtonPressed = true;
                this.VolumeUpButton.IsEnabled = true;
                this.VolumeDownButton.IsEnabled = true;

            }
            else
            {
                MessageBox.Show("File not found: " + path);
            }
        }

        private bool stoppedByUser = false;

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

            stoppedByUser = true; // Set the flag indicating that the user stopped the playback intentionally

                // Enable Continue button if playback was stopped (not finished)
            
                // Stop the playback
                waveOut.Stop();
                StopProgressBarUpdater();

               // Disable Continue button until the user presses Play again
               this.ContinueButton.IsEnabled = true;

                // Enable Play button, and disable Stop button
                this.PlayButton.IsEnabled = false;
                this.StopButton.IsEnabled = false;
                this.forwardButton.IsEnabled = false;
                this.backwardButton.IsEnabled = false;

                // Disable volume buttons when playback is stopped
                this.VolumeUpButton.IsEnabled = false;
                this.VolumeDownButton.IsEnabled = false;



        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            // Continue playing from the current position
            waveOut.Play();
            StartProgressBarUpdater();

            // Enable Stop button, and disable Play and Continue buttons
            this.StopButton.IsEnabled = true;
            this.PlayButton.IsEnabled = false;
            this.ContinueButton.IsEnabled = false;
            this.forwardButton.IsEnabled = true;
            this.backwardButton.IsEnabled = true;

            // Disable volume buttons when playback continues
            this.VolumeUpButton.IsEnabled = true;
            this.VolumeDownButton.IsEnabled = true;

        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            // Reset the current time to zero before stopping playback
            audioFileReader.CurrentTime = TimeSpan.Zero;

            // Stop playback immediately when Cancel is clicked
            waveOut.Stop();
            StopProgressBarUpdater();


            // Reset the progress bar to the start position
            progressBar.Value = 0;


            // Enable Play button, and disable Stop and Continue buttons
            this.PlayButton.IsEnabled = true;
            this.StopButton.IsEnabled = false;
            this.ContinueButton.IsEnabled = false;
            this.forwardButton.IsEnabled = false;
            this.backwardButton.IsEnabled = false;

            this.VolumeUpButton.IsEnabled = false;
            this.VolumeDownButton.IsEnabled = false;

        }

        private void SkipForward_Click(object sender, RoutedEventArgs e)
        {
            // Skip forward by 30 seconds
            audioFileReader.CurrentTime += TimeSpan.FromSeconds(20);
        }

        private void SkipBackward_Click(object sender, RoutedEventArgs e)
        {
            // Skip backward by 30 seconds
            audioFileReader.CurrentTime -= TimeSpan.FromSeconds(20);

            // Ensure we don't go below zero
            if (audioFileReader.CurrentTime < TimeSpan.Zero)
            {
                audioFileReader.CurrentTime = TimeSpan.Zero;
            }

        }

        private void progressBar_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Calculate the new position based on the mouse click
            double mouseClickPosition = e.GetPosition(progressBar).X;
            double totalWidth = progressBar.ActualWidth;
            double newPosition = (mouseClickPosition / totalWidth) * progressBar.Maximum;

            // Set the current time to the new position
            audioFileReader.CurrentTime = TimeSpan.FromSeconds(newPosition);
        }

        

        private void BackButton_clik(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            NavigationService?.GoBack();
        }


        // Get the NavigationService from the hosting window
        private NavigationService NavigationService
        {
            get
            {
                if (NavigationService.GetNavigationService(this) is NavigationService navigationService)
                    return navigationService;

                return null;
            }
        }

        private void VolumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable volume buttons only if the Play button was pressed and audio is not finished
            if (playButtonPressed && waveOut != null && waveOut.PlaybackState != PlaybackState.Stopped
                && (audioFileReader == null || (audioFileReader.TotalTime - audioFileReader.CurrentTime).TotalSeconds > 1))
            {
                waveOut.Volume = Math.Min(1.0f, waveOut.Volume + 0.1f);
            }
        }

        private void VolumeDownButton_Click(object sender, RoutedEventArgs e)
        {
            // Enable volume buttons only if the Play button was pressed and audio is not finished
            if (playButtonPressed && waveOut != null && waveOut.PlaybackState != PlaybackState.Stopped
                && (audioFileReader == null || (audioFileReader.TotalTime - audioFileReader.CurrentTime).TotalSeconds > 1))
            {
                waveOut.Volume = Math.Max(0.0f, waveOut.Volume - 0.1f);
            }
        }

    }
}
