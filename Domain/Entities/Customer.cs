namespace Domain.Entities
{
    public class Customer : Entity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        // Navigation property for the related Books
        public List<Loan> Loans { get; set; } = [];
        public List<Reservation> Reservations { get; set; } = [];
    }
}