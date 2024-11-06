using Microsoft.AspNetCore.Mvc;
using VinylApplication326.DAO;
using VinylApplication326.Models;
using VinylApplication326.Services;

namespace VinylApplication326.Controllers
{
    public class BrowseController : Controller
    {
        private List<RecordModel> records;
        RecordDataService rds = new RecordDataService();

        public IActionResult Index()
        {
            records = rds.readRecords();

            ViewBag.records = records;

            return View();
        }

        [HttpPost]
        public IActionResult FavoriteRecord(int recordId)
        {
            rds.favoriteRecord(recordId);

            return RedirectToAction("Index"); 
        }


    }
}
