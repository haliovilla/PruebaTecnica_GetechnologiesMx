namespace API.DTO
{
    public class PersonDTO : DTOBase
    {
        public string FirstName { get; set; }
        public string PaternalLastname { get; set; }
        public string? MaternalLastname { get; set; }
        public string Identification { get; set; }
        public ICollection<InvoiceDTO> Invoices { get; set; }
    }
}
