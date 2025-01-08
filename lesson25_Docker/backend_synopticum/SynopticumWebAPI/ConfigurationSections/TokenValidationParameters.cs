namespace SynopticumWebAPI.ConfigurationSections
{
    public class TokenValidationParametersConfig
    {
        public string Authority { get; set; }
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}
