using VinylApplication326.DAO;
using VinylApplication326.Models;

namespace VinylApplication326.Business
{
    public class LoginBusiness
    {
        public LoginDAO loginDAO = new LoginDAO();

        public void AuthenticateUser(UserModel user)
        {
            loginDAO.AuthenticateUser(user);
        }

        public void RegisterNewUser(UserModel user)
        {
            loginDAO.RegisterNewUser(user);
        }
    }
}
