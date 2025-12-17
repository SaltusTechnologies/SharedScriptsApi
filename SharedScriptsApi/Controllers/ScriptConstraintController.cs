using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedScriptsApi.Controllers
{
    public class ScriptConstraintController : Controller
    {
        // GET: ScriptConstraintController
        public IActionResult Index()
        {
            return View();
        }

        // GET: ScriptConstraintController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: ScriptConstraintController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScriptConstraintController/Create
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

        // GET: ScriptConstraintController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScriptConstraintController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: ScriptConstraintController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScriptConstraintController/Delete/5
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
