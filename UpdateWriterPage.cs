using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ApiLibraryService;
using System.Windows;

namespace WpfLibrary
{
    public class UpdateWriterPage : PagePerson
    {
        public static GenreList GenresParameters { get; set; }

        public static Writers WriterParameter { get; set; }


        public UpdateWriterPage(Model.GenreList genres)
        {
            GenresParameters = genres;

            Writers writerUpdated = WriterParameter;

            this.ToLoginPage.Visibility = System.Windows.Visibility.Collapsed;
            this.NamePage.Text = "עדכון סופר";
            this.txtCity.Text = ": ז'אנר";
            this.txtUserName.Text = ": ביוגרפיה";
            this.txtPass.Visibility = System.Windows.Visibility.Collapsed;
            this.passWordTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.txtEmail.Visibility = System.Windows.Visibility.Collapsed;
            this.emailTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.txtPhoneNumber.Visibility = System.Windows.Visibility.Collapsed;
            this.txtSplitPhone.Visibility = System.Windows.Visibility.Collapsed;
            this.numPhoneAfterPreTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.prePhone.Visibility = System.Windows.Visibility.Collapsed;
            List<string> genresNames = new List<string>();

            foreach (Genre genre in genres)
            {
                genresNames.Add(genre.GenreName);
            }
            this.comboBoxCity.ItemsSource = genresNames;

            int i = 0;
            string genreWriter = WriterParameter.GenreWriting.GenreName;

            foreach (Genre genre in genres)
            {
                if (genre.GenreName.Equals(genreWriter))
                {
                    this.comboBoxCity.SelectedIndex = i;
                }
                i++;
            }

            this.firstNameTxtBox.Text = writerUpdated.FirstName;
            this.lastNameTxtBox.Text = writerUpdated.LastName;
            this.datePicker.Text = writerUpdated.DateOfBirth.ToString();
            this.userNameTxtBox.Text = writerUpdated.LinkToBiography;

        }


        public async override void Send(object sender, RoutedEventArgs e)
        {
            Writers writerUpdated = WriterParameter;
            writerUpdated.FirstName = this.firstNameTxtBox.Text;
            writerUpdated.LastName = this.lastNameTxtBox.Text;

            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                writerUpdated.DateOfBirth = (DateTime)selectedDate;
            }
            else
            {
                MessageBox.Show("בבקשה לבחור תאריך לידה");
                return;
            }

            writerUpdated.LinkToBiography = this.userNameTxtBox.Text;

            String? genreSelectedName = this.comboBoxCity.SelectedItem as string;

            if (genreSelectedName != null)
            {
                GenreList geners = GenresParameters;
                foreach (Genre genre in geners)
                {
                    if (genre.GenreName.Equals(genreSelectedName))
                    {
                        writerUpdated.GenreWriting = genre;
                    }
                }
            }

            int x = await api.UpdateAWriter(writerUpdated);
            if(x != 0)
            {
                MessageBox.Show("הסופר עודכן בהצלחה");
            }
            else
            {
                MessageBox.Show("שגיאה");
            }


        }


    }
}
