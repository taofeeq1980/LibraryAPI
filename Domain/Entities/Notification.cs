using Domain.Enum;

namespace Domain.Entities
{
    public class Notification : Entity
    {
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public bool Notify { get; set; }
        public NotificationChannel NotifyBy { get; set; }
    }
}