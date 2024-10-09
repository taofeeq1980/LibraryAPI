namespace Domain.Entities
{
    public class Customer : Entity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string PhoneNumber { get; set; }
        // Navigation property for the related Books
        public ICollection<Loan> Loans { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = []; 
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}