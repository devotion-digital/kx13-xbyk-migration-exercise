using Microsoft.AspNetCore.Mvc;

using TestWebsite.Models;

namespace TestWebsite.Components.ContentHero
{
    public class ContentHeroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ContentPageViewModel page)
        {
            return View(page);
        }
    }
}
