namespace ApplicationServices.Books.Response
{
    public class GetBookResponse
    {
        public Guid? BookId { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsReserved { get; set; }
        public string? ReturnedDate { get; set; }
    }
}
