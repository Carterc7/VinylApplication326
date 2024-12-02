using Microsoft.AspNetCore.Mvc;
using VinylApplication326.Models;
using VinylApplication326.Services;

namespace VinylApplication326.Controllers
{
    public class FavoritesController : Controller
    {
        private List<RecordModel> records;
        RecordDataService rds = new RecordDataService();

        public IActionResult Index()
        {
            records = rds.readFavoriteRecords();

            ViewBag.records = records;
            return View();
        }

        public ActionResult EditFavorite(int id)
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
                ViewBag.records = rds.readFavoriteRecords();
                return View("Index");
            }
            else
            {
                return View("EditFavorite", model);
            }
        }


        public ActionResult CreateFavorite()
        {
            var m = new RecordModel();
            m.Favorite = true;
            return View(m);
        }

        public ActionResult DoCreate(RecordModel model)
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            model.UsersId = int.Parse(userIdString);
            bool success = rds.createRecord(model);
            if (success)
            {
                ViewBag.records = rds.readFavoriteRecords();
                return View("Index");
            }
            else
            {
                ViewBag.records = rds.readFavoriteRecords();
                return View("Index");
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
    }
}
