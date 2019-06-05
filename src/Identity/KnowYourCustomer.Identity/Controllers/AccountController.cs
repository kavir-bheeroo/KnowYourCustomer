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
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync(model.Username);
            //        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

            //        if (context != null)
            //        {
            //            if (await _clientStore.IsPkceClientAsync(context.ClientId))
            //            {
            //                // if the client is PKCE then we assume it's native, so this change in how to
            //                // return the response is for better UX for the end user.
            //                return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
            //            }

            //            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            //            return Redirect(model.ReturnUrl);
            //        }

            //        // request for a local page
            //        if (Url.IsLocalUrl(model.ReturnUrl))
            //        {
            //            return Redirect(model.ReturnUrl);
            //        }
            //        else if (string.IsNullOrEmpty(model.ReturnUrl))
            //        {
            //            return Redirect("~/");
            //        }
            //        else
            //        {
            //            // user might have clicked on a malicious link - should be logged
            //            throw new Exception("invalid return URL");
            //        }
            //    }

            //    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
            //    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            //}

            //// something went wrong, show form with error
            //var vm = await BuildLoginViewModelAsync(model);
            //return View(vm);
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