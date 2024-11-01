using VinylApplication326.DAO;
using VinylApplication326.Models;

namespace VinylApplication326.Business
{
    public class LoginBusiness
    {
        public LoginDAO loginDAO = new LoginDAO();

        public UserModel AuthenticateUser(UserModel user)
        {
            return loginDAO.AuthenticateUser(user);
        }

        public bool RegisterNewUser(UserModel user)
        {
            return loginDAO.RegisterNewUser(user);
        }
    }
}
