using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedScriptsApi.Controllers
{
    public class ScriptController : Controller
    {
        // GET: ScriptController
        public IActionResult Index()
        {
            return View();
        }

        // GET: ScriptController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScriptController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScriptController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScriptController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScriptController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScriptController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScriptController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
