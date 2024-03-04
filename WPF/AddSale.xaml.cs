using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPF.DTO;

namespace WPF
{
    public partial class AddSale : Window
    {
        private HttpClient httpClient = new HttpClient();
        private string baseUrl = "https://localhost:7090/api/Sale";
        private readonly int personId;

        public InvoiceDTO NewInvoice { get; private set; }

        public AddSale(int personId)
        {
            InitializeComponent();
            this.personId = personId;
            date.SelectedDate = DateTime.Today;
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(date.Text) &&
                !String.IsNullOrEmpty(amount.Text))
            {
                if (DateTime.TryParse(date.Text, out DateTime invoiceDate))
                {
                    if (decimal.TryParse(amount.Text, out decimal invoiceAmount))
                    {
                        loader.Visibility = Visibility.Visible;

                        var createDto = new CreateInvoiceDTO
                        {
                            Date = invoiceDate,
                            Amount = invoiceAmount,
                            PersonId = personId
                        };

                        try
                        {
                            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/StoreInvoice", createDto);

                            // validate if response status is not bad request

                            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                            {
                                var badRequestResponse = await response.Content.ReadAsStringAsync();
                                MessageBox.Show(badRequestResponse);
                                loader.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                response.EnsureSuccessStatusCode();

                                var jsonString = await response.Content.ReadAsStringAsync();

                                NewInvoice = JsonConvert.DeserializeObject<InvoiceDTO>(jsonString);

                                MessageBox.Show("Venta guardada.");

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
                        MessageBox.Show("El formato del monto es inválido.");
                    }   
                }
                else
                {
                    MessageBox.Show("El formato de fecha es inva´lido.");
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

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
