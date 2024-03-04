using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using WPF.DTO;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private HttpClient httpClient = new HttpClient();
        private string baseUrl = "https://localhost:7090/api/Directory";

        public List<PersonDTO> PeopleList { get; private set; } = new List<PersonDTO>();
        public PersonDTO SelectedPerson { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            LoadData();

            HideDetailsPage();
        }

        private async void LoadData()
        {
            try
            {
                loader.Visibility = Visibility.Visible;

                await Task.Delay(5000);

                var response = await httpClient.GetAsync($"{baseUrl}/FindPeople");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                PeopleList = JsonConvert.DeserializeObject<List<PersonDTO>>(jsonString);

                peopleListView.ItemsSource = PeopleList;

                loader.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}");
            }
        }

        private void peopleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (peopleListView.SelectedItem != null)
            {
                SelectedPerson = (PersonDTO)peopleListView.SelectedItem;

                GetPerson(SelectedPerson.Identification);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddPerson addPersonWindow = new AddPerson();
            if (addPersonWindow.ShowDialog() == true)
            {
                PeopleList.Add(addPersonWindow.NewPerson);
                peopleListView.Items.Refresh();

                addPersonWindow.Close();
            }
        }

        private async void GetPerson(string identification)
        {
            try
            {
                loader.Visibility = Visibility.Visible;

                var response = await httpClient.GetAsync($"{baseUrl}/FindPersonByIdentification/{identification}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var person = JsonConvert.DeserializeObject<PersonDTO>(jsonString);

                var detailsPage = new PersonDetailsPage(person);
                detailsFrame.Navigate(detailsPage);

                loader.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}");
                loader.Visibility = Visibility.Collapsed;
            }
        }

        private void deletePersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPerson != null)
            {
                var confirm = MessageBox.Show($"¿Está seguro de que deseas eliminar a esta persona?\n\n{SelectedPerson.Fullname}",
                "Confirmación de eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                    DeletePerson();
            }
        }

        private async void DeletePerson()
        {
            try
            {
                loader.Visibility = Visibility.Visible;

                var response = await httpClient.DeleteAsync($"{baseUrl}/DeletePersonByIdentification/{SelectedPerson.Identification}");

                response.EnsureSuccessStatusCode();

                PeopleList.Remove(SelectedPerson);

                peopleListView.Items.Refresh();

                MessageBox.Show($"Se eliminó a {SelectedPerson.Fullname}");

                SelectedPerson = null;

                HideDetailsPage();

                loader.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}");
                loader.Visibility = Visibility.Collapsed;
            }
        }

        private void HideDetailsPage() =>
            detailsFrame.Navigate(new NoPersonSelectedPage());
    }
}