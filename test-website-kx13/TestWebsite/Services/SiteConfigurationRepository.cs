using System.Linq;
using System.Threading.Tasks;

using CMS.DocumentEngine.Types.TW;
using CMS.SiteProvider;

using TestWebsite.Models;

namespace TestWebsite.Services
{
    public class SiteConfigurationRepository : ISiteConfigurationRepository
    {
        public Task<SiteConfigurationViewModel> GetAsync()
        {
            var configurationPage = SiteConfigurationProvider.GetSiteConfigurations()
                .OnSite(SiteContext.CurrentSiteName)
                .TopN(1)
                .FirstOrDefault();

            if (configurationPage == null)
            {
                return Task.FromResult(SiteConfigurationViewModel.Empty);
            }

            var model = new SiteConfigurationViewModel
            {
                LogoUrl = configurationPage.SiteConfigurationLogo,
                FooterText = configurationPage.SiteConfigurationFooterText,
            };

            return Task.FromResult(model);
        }
    }
}
