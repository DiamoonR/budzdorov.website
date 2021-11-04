using health.Options;
using health.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace health.Controllers
{    
    [ApiController]
    [Produces("application/json")]
    public class AccountApiController : ControllerBase
    {
        private readonly IOptions<UserOptions> _userOptions;

        public AccountApiController(IOptions<UserOptions> userOptions)
        {
            _userOptions = userOptions;
        }

        [HttpGet("api/account/test")]
        public IActionResult GetTest()
        {
            return Ok("test ok");
        }

        [HttpPost("api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok("invalid");
            }
            if (_userOptions.Value.Login == model.Login && _userOptions.Value.Password == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,model.Login)
                };
                var claimIdentity = new ClaimsIdentity(claims, "Cookie");
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync("Cookie", claimPrincipal);
                return Ok("success");
            }
            else
            {
                return Ok("no success");
            }
        }
    }
}
