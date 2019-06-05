using KnowYourCustomer.Common;
using KnowYourCustomer.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowYourCustomer.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = Guard.IsNotNull(userManager, nameof(userManager));
            _signInManager = Guard.IsNotNull(signInManager, nameof(signInManager));
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: true);
                return result.Succeeded;
            }

            return false;
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new ApplicationUser { UserName = model.Username }, model.Password);
                return result.Succeeded;
            }

            return false;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}