using EDM.Common;

namespace EDM.Entities
{
    public class Invoice : EntityBase
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
