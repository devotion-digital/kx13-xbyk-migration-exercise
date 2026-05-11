namespace TestWebsite.Models
{
    public class SiteConfigurationViewModel
    {
        public string? LogoUrl { get; set; }
        public string? FooterText { get; set; }

        public static SiteConfigurationViewModel Empty => new();
    }
}
