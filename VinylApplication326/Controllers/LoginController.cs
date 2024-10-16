using Microsoft.AspNetCore.Mvc;
using VinylApplication326.Models;

namespace VinylApplication326.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Process the login and display success or failure page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>IActionResult</returns>
        public IActionResult ProcessLogin(UserModel user)
        {
            if (user.UserName == "root" && user.Password == "root")
            {
                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure", user);
            }
        }
    }
}
