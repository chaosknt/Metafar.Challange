using Microsoft.Extensions.Configuration;

namespace Metafar.Challange.Common
{
    public class AppSettingsConfigurationService
    {
        private readonly IConfiguration configuration;

        public AppSettingsConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int TokenLifespan => this.configuration.GetValue<int>("Settings:TokenLifespan", 3);
    }
}