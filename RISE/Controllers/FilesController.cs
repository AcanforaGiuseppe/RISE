using Microsoft.AspNetCore.Mvc;

namespace RISE.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Rules()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "rules.pdf");

            if(!System.IO.File.Exists(path))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/pdf", "RISE_Rules.pdf");
        }

    }
}