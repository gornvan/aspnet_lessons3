namespace FriendsManager.MVC.Security
{
    public static class CorsPolicyBuilder
    {
        public static void AddMvcCrutchCorsPolicy(this WebApplicationBuilder builder, string policyName)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins("http://localhost") // Replace with allowed origin
                          .WithMethods("POST")
                          .WithHeaders("my-mvc-security-crutch-header")
                          .AllowCredentials();
                });
            });
        }

    }
}
