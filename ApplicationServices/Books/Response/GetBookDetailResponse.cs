using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Books.Response
{
    public class GetBookDetailResponse
    {
        public Guid? BookId { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsReserved { get; set; }
        public string? DateBorrowed { get; set; }
        public string? ReturnedDate { get; set; }
        public int NoOfDays { get; set; }
        public BorrowedBy? BorrowedBy { get; set; }
        public BorrowedBy? ReservedBy { get; set; }
    }

    public class BorrowedBy
    {
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}