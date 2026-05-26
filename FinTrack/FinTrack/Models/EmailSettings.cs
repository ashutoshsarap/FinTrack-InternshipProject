namespace FinTrack.Models
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPassword { get; set; }
    }
}
