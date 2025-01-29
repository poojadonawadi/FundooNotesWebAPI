using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public Users Registration(RegisterModel model);

        public string Login(LoginModel login);

        public bool MailExist(string email);

        public ForgotPasswordModel ForgetPassword(string email);
    }
}
