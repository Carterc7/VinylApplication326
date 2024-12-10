using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
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
            var records = rds.readRecords(); // Retrieve the records
            foreach (var record in records)
            {
                Console.WriteLine($"Record ID: {record.Id}, Name: {record.Name}, Favorite: {record.Favorite}, Video: {record.Video}");
            }
            ViewBag.records = records; // Pass the records to the View

            return View();
        }

        [HttpPost]
        public JsonResult FavoriteRecord(int recordId)
        {
            try
            {
                rds.favoriteRecord(recordId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult CreateRecord()
        {
            return View();
        }

        public ActionResult DoCreate(RecordModel model)
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            model.UsersId = int.Parse(userIdString);
            bool success = rds.createRecord(model);
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteRecord(int id)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            bool success = rds.deleteRecord(id, userId);
            if (success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult EditRecord(int id)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            RecordModel model = rds.getRecordByIdAndUserId(id, userId);
            
             return View(model);
            
        }

        public ActionResult DoEdit(RecordModel model)
        {
            bool success = rds.doEdit(model);
            if (success)
            {
                ViewBag.records = rds.readRecords();
                return RedirectToAction("Index");
            }
            else
            {
                return View("EditRecord", model);
            }
        }



    }
}
