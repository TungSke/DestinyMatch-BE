using Microsoft.AspNetCore.Mvc;

namespace DestinyMatch_API.Controllers
{
    public class ConversationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
