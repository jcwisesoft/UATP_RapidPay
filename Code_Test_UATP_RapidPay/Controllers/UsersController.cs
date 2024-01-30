using Code_Test_UATP_RapidPay.Models.Entities;
using Code_Test_UATP_RapidPay.Models.RequestModels;
using Code_Test_UATP_RapidPay.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Code_Test_UATP_RapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseAPIController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return  Ok(users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserModel model)
        {
            await _userService.CreateUser(model);

            return Ok("User created successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserModel model)
        {
            try
            {
                dynamic response = await _userService.Authenticate(model);
                var result = new { data = response, message = "User login was successfully" };

                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

    }
}
