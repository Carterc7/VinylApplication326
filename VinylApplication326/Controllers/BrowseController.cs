﻿using Microsoft.AspNetCore.Mvc;
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
