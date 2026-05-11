using System.Threading.Tasks;

using TW.Web.Models;

namespace TW.Web.Services;

public interface ISiteConfigurationRepository
{
    Task<SiteConfigurationViewModel> GetAsync();
}
