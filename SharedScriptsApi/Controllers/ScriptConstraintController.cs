using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saltus.digiTICKET.Data0111000000.Models;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Controllers
{
    public class ScriptConstraintController : Controller
    {
        [HttpGet("ScriptConstraints/Core")]
        public IActionResult GetScriptConstraintsCore() { return View(); }

        [HttpGet("ScriptConstraints/OK")]
        public IActionResult GetScriptConstraintsOK() { return View(); }
        [HttpPost("ScriptConstraints/Core/Add")]
        public IActionResult AddScriptConstraintCore(IScriptConstraint scriptConstraint)
        {
            return View(scriptConstraint); 
        }
        [HttpPost("ScriptConstraints/OK/Add")]
        public IActionResult AddScriptConstraintOK(IScriptConstraint scriptConstraint)
        {
            return View(scriptConstraint);
        }
        [HttpPost("ScriptConstraints/Core/Update")]
        public IActionResult UpdateScriptConstraintCore(IScriptConstraint scriptConstraint)
        {
            return View(scriptConstraint);
        }
        [HttpPost("ScriptConstraints/OK/Update")]
        public IActionResult UpdateScriptContraintOK(IScriptConstraint scriptConstraint)
        {
            return View(scriptConstraint);
        }
      
    }
}
