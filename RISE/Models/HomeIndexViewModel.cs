/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
namespace RISE.Models
{
    public class HomeIndexViewModel
    {
        public List<string> SliderImages { get; set; } = new();
        public List<News> LatestNews { get; set; } = new();
    }
}