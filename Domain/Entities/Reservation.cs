namespace Domain.Entities
{
    public class Reservation: Entity
    {
        public Guid? CustomerId { get; set; }    
        public Customer? Customer { get; set; }
        public Guid? BookId { get; set; }
        public Book? Book { get; set; }
    }
}