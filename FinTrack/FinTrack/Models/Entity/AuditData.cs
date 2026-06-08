namespace FinTrack.Models.Entity
{
    public class AuditData
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? RequestPath { get; set; }
        public string? Method { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
