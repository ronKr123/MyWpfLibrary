using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfLibrary
{
    public class UpdateGenrePage : AddGenrePage
    {
        public static Genre GenreParameter { get; set; }


        // לפני המעבר לדף זה , צריך להתחל את הפרמטר Genre לדף
        // UpdateGenrePage.GenreParameter = genre...;


        public UpdateGenrePage(GenreList genres) : base(genres)
        {
            this.TitlePageGenre.Text = "עדכון ז'אנר";
            this.addGenre.Content = "עדכון";
            if (GenreParameter != null)
            {
                this.genreNameTxtBox.Text = GenreParameter.GenreName;
            }

        }


        public override async void addGenre_Click(object sender, RoutedEventArgs e)
        {
            Genre genreUpdated = GenreParameter;

            string genreName = this.genreNameTxtBox.Text;

            if ((genreName != null) && (!ExistGenre(genreName)))
            {
                genreUpdated.GenreName = genreName;
                int x = await api.UpdateAGenre(genreUpdated);
                if (x != 0)
                {
                    MessageBox.Show("הז'אנר עודכן בהצלחה");
                    this.genreNameTxtBox.Text = "";
                    GenersAllList = await api.SelectAllGeneres();
                }
            }
            else
            {
                if (ExistGenre(genreName))
                {
                    MessageBox.Show("כבר קיים ז'אנר בשם זה");
                }
                if (genreName == null)
                {
                    MessageBox.Show("לא הוכנסו נתונים");
                }
            }
        }




        // Checking if there is a Genre with the same name, (if exist)
        private bool ExistGenre(string genreName)
        {
            GenreList genres = GenersAllList;
            foreach (Genre genre in genres)
            {
                if (genre.GenreName.Equals(genreName))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
