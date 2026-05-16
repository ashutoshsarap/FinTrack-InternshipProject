namespace FinTrack.CustomExceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException() : base("A record with the same details already exists.")
        {
        }
        public DuplicateRecordException(string message) : base(message)
        {
        }
        public DuplicateRecordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
