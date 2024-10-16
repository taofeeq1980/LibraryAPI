﻿namespace Domain.Entities
{
    public class Book: Entity
    {
        public required string ISBN { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsReserved { get; set; }
        public DateTime ReturnedDate { get; set; } 
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}