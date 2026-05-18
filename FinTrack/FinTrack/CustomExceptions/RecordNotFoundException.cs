namespace FinTrack.CustomExceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base("The requested record was not found.")
        {
        }
        public RecordNotFoundException(string message) : base(message)
        {
        }
        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
