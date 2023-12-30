using ApiLibraryService;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UsersListPage.xaml
    /// </summary>
    public partial class UsersListPage : Page
    {
        ApiService apiService = new ApiService();
        public static List<Users> Users { get; set; }

        private Users selectedUser;


        private UsersList usersList;

        public UsersListPage()
        {
            InitializeComponent();
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            List<Users> userList = await GetUsers();
            //DataContext = userList;
            this.myListView.ItemsSource = userList;
        }

        private async Task<List<Users>> GetUsers()
        {
            UsersList fetchedUsersList = await apiService.SelectAllUsers();
            return fetchedUsersList.ToList(); // Assuming fetchedUsersList is IEnumerable<Users>
        }


        private void userListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedUser = item.DataContext as Users;
            }
        }

        private async void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                // Navigate to the update page with the selected user

                CityList cities = await apiService.SelectAllCities();
                UpdateUserPage.Userr = selectedUser;
                UpdateUserPage updateUserPage = new UpdateUserPage(cities);

                Frame frame = FrameHolder.MangerFrame;
                frame.Navigate(updateUserPage);
            }
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                // Call your API method to delete the user
                int result = await apiService.DeleteAUser(selectedUser);

                if (result > 0)
                {
                    // If the deletion was successful, update the user list
                    UsersList usersList = await apiService.SelectAllUsers();

                    // Refresh the ListView
                    this.myListView.ItemsSource = null;
                    this.myListView.ItemsSource = usersList;
                }
                else
                {
                    MessageBox.Show("מחיקת המשתמש נכשלה");
                }
            }
        }

        private void userListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListViewItem item)
            {
                // Get the selected User from the DataContext of the ListViewItem
                selectedUser = item.DataContext as Users;
            }
        }

        private void myListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(myListView.SelectedItem != null)
            {
                selectedUser = myListView.SelectedItem as Users;
            }
            else
            {
                selectedUser = null;
            }
        }
    }
}
