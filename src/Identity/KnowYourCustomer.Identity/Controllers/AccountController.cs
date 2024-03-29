﻿using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Exceptions;
using KnowYourCustomer.Identity.Contracts.Models;
using KnowYourCustomer.Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = Guard.IsNotNull(userManager, nameof(userManager));
            _signInManager = Guard.IsNotNull(signInManager, nameof(signInManager));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: true);
                return result.Succeeded;
            }

            return Ok(false);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseModel>> Register(RegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(new ApplicationUser { UserName = model.Username }, model.Password);
                var user = await _userManager.FindByNameAsync(model.Username);

                return new RegisterResponseModel { UserId = user.Id, Username = user.UserName };
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult> Update(Guid userId, UpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        // Added just for testing purposes.
        [AllowAnonymous]
        [HttpGet("testexception")]
        public Task<IActionResult> TestException()
        {
            throw new ObjectAlreadyExistsException("Testing the response middleware");
        }
    }
}