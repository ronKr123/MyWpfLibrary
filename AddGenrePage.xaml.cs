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

namespace WpfLibrary
{
    /// <summary>
    /// Interaction logic for AddGenrePage.xaml
    /// </summary>
    public partial class AddGenrePage : Page
    {

        public static ApiService api = new ApiService();

        public static GenreList GenersAllList { get; set; }

        public AddGenrePage(Model.GenreList genres)
        {
            InitializeComponent();
            this.genreNameTxtBox.Text = "";
            if(genres != null)
            {
                GenersAllList = genres;
            }

        }

        public virtual async void addGenre_Click(object sender, RoutedEventArgs e)
        {
            string genreName = this.genreNameTxtBox.Text;
            if ((genreName != null) && (!ExistGenre(genreName)))
            {
                Genre newGenre = new Genre();
                newGenre.GenreName = genreName;
                int x = await api.InsertAGenre(newGenre);
                if (x != 0)
                {
                    MessageBox.Show("הז'אנר הוסף בהצלחה");
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
