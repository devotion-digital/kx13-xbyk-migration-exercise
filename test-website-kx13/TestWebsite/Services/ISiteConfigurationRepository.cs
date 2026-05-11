using System.Threading.Tasks;

using TestWebsite.Models;

namespace TestWebsite.Services
{
    public interface ISiteConfigurationRepository
    {
        Task<SiteConfigurationViewModel> GetAsync();
    }
}
