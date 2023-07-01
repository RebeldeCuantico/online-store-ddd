namespace ReverseProxy.Settings
{
    public class OpenIdConnectSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
        public string Discovery { get; set; }

        public string DiscoveryUrl
        {
            get
            {
                return $"{Authority}/{Discovery}";
            }
        }
    }
}
