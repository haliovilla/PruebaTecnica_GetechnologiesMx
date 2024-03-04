namespace WPF.DTO
{
    public class InvoiceDTO : DTOBase
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int PersonId { get; set; }
        public PersonDTO Person { get; set; }
    }
}
