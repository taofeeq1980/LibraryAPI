using Domain.Enum;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Command
{
    public class NotifyAboutBookCommand : IRequest<Result>
    {
        public Guid BookId { get; set; }
        public NotificationChannel NotificationChannel { get; set; }
    }
}