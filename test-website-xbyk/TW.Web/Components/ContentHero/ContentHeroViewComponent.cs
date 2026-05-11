using Microsoft.AspNetCore.Mvc;

using TW.Web.Models;

namespace TW.Web.Components.ContentHero;

public class ContentHeroViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ContentPageViewModel page)
    {
        return View(page);
    }
}
