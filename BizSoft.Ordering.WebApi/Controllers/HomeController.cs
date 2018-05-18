using Microsoft.AspNetCore.Mvc;

namespace Ordering.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET api/values
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult( "~/swagger" );
        }
    }
}
