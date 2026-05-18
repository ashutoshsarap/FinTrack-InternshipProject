namespace FinTrack.CustomExceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException() : base("Amount must be greater than zero.")
        {
        }
        public InvalidAmountException(string message) : base(message)
        {
        }
        public InvalidAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
