namespace FinTrack.Models.Entity
{
    public class AuditData
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string? EntityActedUpon { get; set; }
        public int EntityId { get; set; }

    }
}
