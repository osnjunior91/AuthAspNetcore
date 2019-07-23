namespace ProductCatalog.Api.Security
{
    public class JwtConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string JwtKey { get; set; }
        public int Days { get; set; }
        public int Minutes { get; set; }
    }
}
