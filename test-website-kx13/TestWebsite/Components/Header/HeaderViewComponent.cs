using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TestWebsite.Services;

namespace TestWebsite.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISiteConfigurationRepository siteConfigurationRepository;

        public HeaderViewComponent(ISiteConfigurationRepository siteConfigurationRepository)
        {
            this.siteConfigurationRepository = siteConfigurationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configuration = await siteConfigurationRepository.GetAsync();
            return View(configuration);
        }
    }
}
