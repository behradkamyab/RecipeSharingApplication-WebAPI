using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Interfaces;
using SharedModelsLibrary.UserRequests;



namespace RecipeSharingWebApi.Controllers
{

    // change password

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;


        public AuthController(IUserManager userManager)
        {
            _userManager = userManager;
        }



        //POST /api/Auth/Register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest userModel)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Check the properties!");
                }

                var result = await _userManager.RegisterUserASync(userModel);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);  
        }



        //Post /api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] SharedModelsLibrary.UserDTOs.UserLoginRequest userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Check the properties!");
            }

            var result = await _userManager.LoginUserAsync(userModel);

            if (!result.Success)
            {
                if(result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
