using System;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RepositoryLayer.Entity;

namespace FundooNotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly IBus bus;

        public UsersController(IUserManager manager, IBus bus)
        {
            this.manager = manager;
            this.bus = bus;
        }

        [HttpPost]
        [Route("Reg")]
        public IActionResult Register(RegisterModel model)
        {
            var checkMail = manager.MailExist(model.Email);
            if (checkMail)
            {
                return BadRequest(new ResponseModel<bool> { Success = true, Message = "Email Already Exist" });
            }
            var result = manager.Registration(model);
            if(result!= null)
            {
                return Ok(new ResponseModel<Users> { Success = true, Message = "Register successfull", Data=result });
            }
            else
            {
                return BadRequest(new ResponseModel<Users> { Success = false, Message = "Register Failed" });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(LoginModel model)
        {
            var response = manager.Login(model);
            if(response != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Login Successful", Data = response});
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "Login Falied" });
        }

        [HttpGet]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                if (manager.MailExist(email))
                {
                    ForgotPasswordModel forgotPasswordModel = manager.ForgetPassword(email);
                    Send send = new Send();
                    send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgotPasswordModel);
                    return Ok(new ResponseModel<string> { Success = true, Message = "Mail sent successfully", Data = endPoint.ToString() });
                }
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Please provide valid email" });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
    
}
