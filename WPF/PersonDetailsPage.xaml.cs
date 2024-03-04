using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WPF.DTO;

namespace WPF
{
    public partial class PersonDetailsPage : Page
    {
        private HttpClient httpClient = new HttpClient();
        private string baseUrl = "https://localhost:7090/api/Directory";

        private readonly PersonDTO person;

        public PersonDetailsPage(PersonDTO person)
        {
            InitializeComponent();
            this.person = person;
            DataContext = person;
            invoicesListView.ItemsSource = person.Invoices;
        }

        private void invoicesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addSaleButton_Click(object sender, RoutedEventArgs e)
        {
            var addSaleWindow = new AddSale(person.Id);

            if (addSaleWindow.ShowDialog() == true)
            {
                person.Invoices.Add(addSaleWindow.NewInvoice);
                invoicesListView.Items.Refresh();

                addSaleWindow.Close();
            }
        }

        
    }
}
