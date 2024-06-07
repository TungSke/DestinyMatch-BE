using Microsoft.AspNetCore.Mvc;

namespace FPT.DestinyMatch.API.Controllers
{
    public class ConversationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
