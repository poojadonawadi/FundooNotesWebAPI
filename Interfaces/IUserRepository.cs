using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public Users Registration(RegisterModel model);

        public string Login(LoginModel login);

        public ForgotPasswordModel ForgetPassword(string email);
    }
}
