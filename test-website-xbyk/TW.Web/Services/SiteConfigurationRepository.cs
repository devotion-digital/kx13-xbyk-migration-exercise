using System.Linq;
using System.Threading.Tasks;

using Kentico.Content.Web.Mvc;

using TW.Web.Generated;
using TW.Web.Models;

namespace TW.Web.Services;

public class SiteConfigurationRepository : ISiteConfigurationRepository
{
    private readonly IContentRetriever contentRetriever;

    public SiteConfigurationRepository(IContentRetriever contentRetriever)
    {
        this.contentRetriever = contentRetriever;
    }

    public async Task<SiteConfigurationViewModel> GetAsync()
    {
        var pages = await contentRetriever.RetrievePages<SiteConfiguration>(
            new RetrievePagesParameters { LinkedItemsMaxLevel = 1 });

        var config = pages.FirstOrDefault();
        if (config == null)
        {
            return SiteConfigurationViewModel.Empty;
        }

        return new SiteConfigurationViewModel
        {
            LogoUrl = MediaHelper.FirstUrl(config.SiteConfigurationLogo),
            FooterText = config.SiteConfigurationFooterText,
        };
    }
}
