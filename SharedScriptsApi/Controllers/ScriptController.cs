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
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ScriptController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScriptController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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
        public IActionResult Edit(int id)
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
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScriptController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
