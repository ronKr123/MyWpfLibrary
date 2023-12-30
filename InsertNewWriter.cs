using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfLibrary
{
    public class InsertNewWriter : PagePerson
    {
        public static GenreList GenresParameters { get; set; }

        public InsertNewWriter(Model.GenreList genres)
        {
            GenresParameters = genres;

            this.ToLoginPage.Visibility = System.Windows.Visibility.Collapsed;
            this.NamePage.Text = "הוספת סופר חדש";
            this.txtCity.Text = ": ז'אנר";
            this.txtCity.Visibility = Visibility.Visible;
            this.comboBoxCity.Visibility = Visibility.Visible;

            this.txtUserName.Text = "ביוגרפיה";
            this.txtUserName.Visibility = Visibility.Visible;
            this.userNameTxtBox.Visibility = Visibility.Visible;

            this.txtPass.Visibility = System.Windows.Visibility.Collapsed;
            this.passWordTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.txtEmail.Visibility = System.Windows.Visibility.Collapsed;
            this.emailTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.txtPhoneNumber.Visibility = System.Windows.Visibility.Collapsed;
            this.txtSplitPhone.Visibility = System.Windows.Visibility.Collapsed;
            this.numPhoneAfterPreTxtBox.Visibility = System.Windows.Visibility.Collapsed;
            this.prePhone.Visibility = System.Windows.Visibility.Collapsed;
            List<string> genresNames = new List<string>();
            foreach(Genre genre in genres)
            {
                genresNames.Add(genre.GenreName);
            }
            this.comboBoxCity.ItemsSource = genresNames;


        }

        public override async void Send(object sender, RoutedEventArgs e)
        {
            Writers newWriter = new Writers();
            newWriter.FirstName = this.firstNameTxtBox.Text;
            newWriter.LastName = this.lastNameTxtBox.Text;

            DateTime? selectedDate = datePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                newWriter.DateOfBirth = (DateTime)selectedDate;

            }
            else
            {
                MessageBox.Show("בבקשה לבחור תאריך לידה");
                return;
            }

            String? genreSelectedName = this.comboBoxCity.SelectedItem as string;

            if (genreSelectedName != null)
            {
                GenreList genres = GenresParameters;
                foreach (Genre genre in genres)
                {
                    if (genre.GenreName.Equals(genreSelectedName))
                    {
                        newWriter.GenreWriting = genre;
                    }
                }
            }
            else
            {
                MessageBox.Show("בבקשה לבחור ז'אנר");
                return;
            }

            newWriter.LinkToBiography = this.userNameTxtBox.Text;
            int x = await api.InsertAWriter(newWriter);
            if(x != 0)
            {
                MessageBox.Show("הכותב הוסף בהצלחה");
                this.firstNameTxtBox.Text = "";
                this.lastNameTxtBox.Text = "";
                this.datePicker.Text = "";
                this.comboBoxCity.SelectedItem = null;
                this.userNameTxtBox.Text = "";
            }
            else
            {
                MessageBox.Show("שגיאה בהוספת הכותב");
            }

        }




    }
}
