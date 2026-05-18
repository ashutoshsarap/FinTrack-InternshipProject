namespace FinTrack.CustomExceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException() : base("Date cannot be in the future.")
        {
        }
        public InvalidDateException(string message) : base(message)
        {
        }
        public InvalidDateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
