using Microsoft.AspNetCore.Mvc;

using TestWebsite.Models;

namespace TestWebsite.Components.HomeHero
{
    public class HomeHeroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HomePageViewModel page)
        {
            return View(page);
        }
    }
}
