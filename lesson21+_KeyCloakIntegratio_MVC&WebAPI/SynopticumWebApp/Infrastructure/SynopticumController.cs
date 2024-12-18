using Microsoft.AspNetCore.Mvc;

namespace SynopticumWebApp.Infrastructure
{
    public class SynopticumController: Controller
    {
        public SynopticumController()
        {
            ViewData["Layout"] = "_Layout";
        }
    }
}
