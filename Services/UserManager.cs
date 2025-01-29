using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository user;
        private readonly FundooDBContext context;

        public UserManager(IUserRepository user, FundooDBContext context)
        {
            this.user = user;
            this.context = context;
        }

        public Users Registration(RegisterModel model)
        {
            return user.Registration(model);
        }

        public bool MailExist(string email)
        {
            var checkMailexist = this.context.Users.FirstOrDefault(b => b.Email == email);
            if (checkMailexist != null)
            {
                return true;
            }
            return false;
        }

        public string Login(LoginModel login)
        {
            return user.Login(login);
        }

        public ForgotPasswordModel ForgetPassword(string email)
        {
            return user.ForgetPassword(email);
        }
    }
}
