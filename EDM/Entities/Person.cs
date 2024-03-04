using EDM.Common;

namespace EDM.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }
        public string PaternalLastname { get; set; }
        public string? MaternalLastname { get; set; }
        public string Identification { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
