namespace APLTechChallenge.Models
{
    public class ImageAudit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool OfflineMode { get; set; }
        public bool SentToAzure { get; set; }
        public DateTime DateUploaded { get; set; }
        public string SuccessRecord { get; set; } = string.Empty;
    }
}
