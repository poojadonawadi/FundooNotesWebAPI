using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public UserRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        
        public Users Registration(RegisterModel model)
        {
            Users users = new Users();
            users.FirstName = model.FirstName;
            users.LastName = model.LastName;
            users.DOB = model.DOB;
            users.Gender = model.Gender;
            users.Email = model.Email;
            users.Password = EncodePassword(model.Password);
            context.Users.Add(users);
            context.SaveChanges();
            return users;
        }

        public string Login(LoginModel login)
        {
            var Checkmail = this.context.Users.FirstOrDefault(a => a.Email == login.Email && a.Password == EncodePassword(login.Password));
            if(Checkmail != null)
            {
                string token = GenerateToken(Checkmail.UserId, Checkmail.Email);
                return token;
            }
            return null;

        }
        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        private string GenerateToken(int UserID, string EmailID)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("UserID", UserID.ToString()),
                new Claim("Email", EmailID)
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ForgotPasswordModel ForgetPassword(string email)
        {
            Users user = context.Users.ToList().Find(a => a.Email == email);
            if(user != null)
            {
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                forgotPasswordModel.UserId = user.UserId;
                forgotPasswordModel.Email = user.Email;
                forgotPasswordModel.Token = GenerateToken(user.UserId, user.Email);
                return forgotPasswordModel;
            }
            else
            {
                throw new Exception("User not exists for this email");
            }
        }

    }
}
