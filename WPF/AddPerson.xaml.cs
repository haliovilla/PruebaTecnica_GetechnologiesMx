using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.DTO;

namespace WPF
{
    public partial class AddPerson : Window
    {
        private HttpClient httpClient = new HttpClient();
        private string baseUrl = "https://localhost:7090/api/Directory";

        public PersonDTO NewPerson { get; private set; }

        public AddPerson()
        {
            InitializeComponent();
            
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(firstName.Text) &&
                !String.IsNullOrEmpty(paternalLastname.Text) &&
                !String.IsNullOrEmpty(identification.Text))
            {
                loader.Visibility = Visibility.Visible;

                var createDto = new CreatePersonDTO
                {
                    FirstName = firstName.Text,
                    PaternalLastname = paternalLastname.Text,
                    MaternalLastname = maternalLastname.Text,
                    Identification = identification.Text
                };

                try
                {
                    var response = await httpClient.PostAsJsonAsync($"{baseUrl}/StorePerson", createDto);

                    // validate if response status is not bad request

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var br = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(br);
                        loader.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();

                        var jsonString = await response.Content.ReadAsStringAsync();

                        NewPerson = JsonConvert.DeserializeObject<PersonDTO>(jsonString);

                        MessageBox.Show($"Se agregó a {NewPerson.Fullname}");

                        DialogResult = true;
                    }


                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    loader.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show("Hay campos que estan vacíos");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
