using System.Web.Mvc;

namespace Scheduler.WebConsole.Controllers
{
    public class SchedulerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}