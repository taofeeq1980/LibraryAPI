namespace Domain.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public DateTime? DateAdded { get; set; }
        public bool IsDeleted { get; set; }

        protected Entity()
        {
            DateAdded = DateTime.UtcNow;
            IsDeleted = false;
            //create a new Guid
            Id = Guid.NewGuid();
        }
    }
}