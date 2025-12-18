using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Controllers
{
    public class ScriptController : Controller
    {
        [HttpGet("Scripts/Core")]
        public IActionResult GetCoreScripts()
        {
                       return View();

        }
        [HttpGet("Scripts/OK")]
        public IActionResult GetOKScripts()
        {
            return View();
        }

        [HttpPost("Scripts/Core/Add")]
        public IActionResult AddScriptCore(IScript script)
        {
            return View(script);
        }
        [HttpPost("Scripts/OK/Add")]
        public IActionResult AddScriptOk(IScript script)
        {
            return View(script);
        }
        [HttpPost("Scripts/Core/Update")]
        public IActionResult UpdateScriptCore(IScript script)
        {
            return View(script);
        }
        [HttpPost("Scripts/OK/pdate")]
        public IActionResult UpdateScriptsOK(IScript script)
        {
            return View(script);
        }
    }
}
