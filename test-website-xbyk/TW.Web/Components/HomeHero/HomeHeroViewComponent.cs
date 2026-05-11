using Microsoft.AspNetCore.Mvc;

using TW.Web.Models;

namespace TW.Web.Components.HomeHero;

public class HomeHeroViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HomePageViewModel page)
    {
        return View(page);
    }
}
