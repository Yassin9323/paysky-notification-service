namespace NotificationService.Infrastructure.Logging
{
    public class LoggerSettings
    {
        public string ElasticSearchUrl { get; set; }
        public string ElasticUserName { get; set; }
        public string ElasticPassword { get; set; }
        public string IndexFormat { get; set; }
        public string FilePath { get; set; }
    }
}