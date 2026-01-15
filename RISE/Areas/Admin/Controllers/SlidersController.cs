using Microsoft.AspNetCore.Mvc;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : BaseAdminController
    {
        private readonly IWebHostEnvironment _env;

        public SlidersController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var folder = Path.Combine(_env.WebRootPath, "images", "slider");
            Directory.CreateDirectory(folder);

            var files = Directory.GetFiles(folder)
                                 .Select(Path.GetFileName)
                                 .ToList();

            return View(files);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(List<IFormFile> files)
        {
            if(files == null || files.Count == 0)
                return RedirectToAction(nameof(Index));

            var folder = Path.Combine(_env.WebRootPath, "images", "slider");
            Directory.CreateDirectory(folder);

            foreach(var file in files)
            {
                if(file.Length == 0) continue;

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
                return RedirectToAction(nameof(Index));

            var path = Path.Combine(_env.WebRootPath, "images", "slider", fileName);

            if(System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            return RedirectToAction(nameof(Index));
        }
    }
}
