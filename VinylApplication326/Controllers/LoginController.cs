using Microsoft.AspNetCore.Mvc;
using VinylApplication326.Business;
using VinylApplication326.Models;

namespace VinylApplication326.Controllers
{
    public class LoginController : Controller
    {
        
         


        public IActionResult Index()
        {
            return View("Index", new UserModel());
        }

        /// <summary>
        /// Process the login and display success or failure page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>IActionResult</returns>
        public IActionResult ProcessLogin(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", user);
            }
            try
            {
                LoginBusiness loginBusiness = new LoginBusiness();
                UserModel loggedInUser = loginBusiness.AuthenticateUser(user);
                if (loggedInUser.Id == 0)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return View("LoginFailure");
                }
                
                return View("LoginSuccess");
            }
            catch (Exception ex)
            {
                return View("LoginFailure");

            }
        }

        #region REGISTER

        /// <summary>
        /// Method to show the user the register page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            
            return View("Register", new UserModel());
        }

        /// <summary>
        /// Triggered when a user submits a form to register. Should add new user to database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult ProccessRegister(UserModel user)
        {
            // Model: Username and passowrd only
            if(!ModelState.IsValid)
            {
                return View("Register", user);
            }
            try
            {
                LoginBusiness loginBusiness = new LoginBusiness();
                bool x = loginBusiness.RegisterNewUser(user);
                return View("RegisterSuccess");
            }catch (Exception ex)
            {
                return View("RegisterFailure");

            }
        }
        #endregion REGISTER


    }
}
