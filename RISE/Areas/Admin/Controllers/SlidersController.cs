using Microsoft.AspNetCore.Mvc;

namespace RISE.Areas.Admin.Controllers
{
    public class SlidersController : BaseAdminController
    {
        private readonly IWebHostEnvironment _env;

        public SlidersController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: /Admin/Slider
        public IActionResult Index()
        {
            var folder = Path.Combine(_env.WebRootPath, "images", "slider");
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var files = Directory.GetFiles(folder)
                .Select(f => "/images/slider/" + Path.GetFileName(f))
                .OrderBy(x => x)
                .ToList();

            return View(files);
        }

        // POST: /Admin/Slider/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file.";
                return RedirectToAction(nameof(Index));
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
            if(!allowed.Contains(ext))
            {
                TempData["Error"] = "Only .jpg, .jpeg, .png, .webp are allowed.";
                return RedirectToAction(nameof(Index));
            }

            // Optional: size limit (5 MB)
            const long maxBytes = 5 * 1024 * 1024;
            if(file.Length > maxBytes)
            {
                TempData["Error"] = "File too large. Max 5MB.";
                return RedirectToAction(nameof(Index));
            }

            var folder = Path.Combine(_env.WebRootPath, "images", "slider");
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var safeName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, safeName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            TempData["Success"] = "Image uploaded.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Slider/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
                return RedirectToAction(nameof(Index));

            // prevent path traversal
            fileName = Path.GetFileName(fileName);

            var folder = Path.Combine(_env.WebRootPath, "images", "slider");
            var fullPath = Path.Combine(folder, fileName);

            if(System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            TempData["Success"] = "Image deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
