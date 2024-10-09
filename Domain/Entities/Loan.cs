namespace Domain.Entities
{
    public class Loan : Entity
    {
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid? BookId { get; set; } 
        public int Tenor { get; set; }
    }
}