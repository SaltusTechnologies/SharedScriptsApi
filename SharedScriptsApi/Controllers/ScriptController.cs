using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedScriptsApi.Interfaces;
using SharedScriptsApi.Services;

namespace SharedScriptsApi.Controllers
{
    public class ScriptController : BaseController
    {
        
        public ScriptController(ILogger<ScriptController> logger, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) 
            : base(logger, httpContextAccessor, serviceProvider)
        {
        }

        [HttpGet("Scripts/Core")]
        public async Task<IActionResult> GetCoreScripts()
        {
            var scriptService = _serviceProvider.GetService<IScriptService>();
            if (scriptService == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Script service not available.");
            }

            var scripts = await scriptService.GetScriptsAsync();
            return Json(scripts);
        }

        [HttpGet("Scripts/OK")]
        public async Task<IActionResult> GetOKScripts()
        {
            var scriptService = _serviceProvider.GetService<IScriptService>();
            if (scriptService == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Script service not available.");
            }

            var scripts = await scriptService.GetScriptsAsync();
            return Json(scripts);
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
        [HttpPost("Scripts/OK/Update")]
        public IActionResult UpdateScriptsOK(IScript script)
        {
            return View(script);
        }
    }
}
