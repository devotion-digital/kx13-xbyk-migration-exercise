using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TW.Web.Services;

namespace TW.Web.Components.Footer;

public class FooterViewComponent : ViewComponent
{
    private readonly ISiteConfigurationRepository siteConfigurationRepository;

    public FooterViewComponent(ISiteConfigurationRepository siteConfigurationRepository)
    {
        this.siteConfigurationRepository = siteConfigurationRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var configuration = await siteConfigurationRepository.GetAsync();
        return View(configuration);
    }
}
