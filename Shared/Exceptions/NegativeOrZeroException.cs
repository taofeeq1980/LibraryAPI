namespace Shared.Exceptions
{
    public class NegativeOrZeroException:Exception
    {
        public string Code { get; set; }
        public NegativeOrZeroException(string message, string code="ArgumentNull"):base(message)
        {
            Code = code;
        }
    }
}