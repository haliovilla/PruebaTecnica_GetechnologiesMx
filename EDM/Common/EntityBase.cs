namespace EDM.Common
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
}
