﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerOfControl.Models;
using PowerOfControl.Services;

namespace PowerOfControl.Controllers;

[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly AccountService _authService;

    public AccountController(AccountService authService)
    {
        _authService = authService;
    }

    // GET: api/Account/GetUser
    [Authorize]
    [HttpGet]
    public IActionResult GetUser()
    {
        // Attempt to update the user
        var currentUserTag = User.FindFirst("tag_name")?.Value;

        // Retrieve current user information
        var user = _authService.GetCurrentUser(currentUserTag);
        if (user != null)
        {
            // Create a simplified user DTO for response
            var UserLoginDto = new
            {
                user.tag_name,
                user.user_name,
                user.email,
                user.notifications
            };

            return Ok(new { message = "User data get successful", user = UserLoginDto });
        }
        else
        {
            return Ok(new { message = "User not found" });
        }
    }

    // POST: api/Account/Authorization
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Authorization(User user)
    {
        // Attempt to authorize the user
        if (_authService.AuthorizationUser(user))
        {
            return Ok(new { message = "User authorization successfully" });
        }
        else
        {
            return BadRequest(new { message = "User authorization failed" });
        }
    }

    // POST: api/Account/Login
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login(UserLoginDto user)
    {
        // Attempt to log in the user
        var userToken = _authService.LoginUser(user);

        if (userToken != "")
        {
            return Ok(new { message = "User login successfully", user_token = userToken });
        }
        else
        {
            return BadRequest(new { message = "User login failed" });
        }
    }

    // PATCH: api/Account/UpdateUser
    [Authorize]
    [HttpPatch]
    public IActionResult UpdateUser(UserDto request)
    {
        // Attempt to update the user
        var currentUserTag = User.FindFirst("tag_name").Value;

        var userToken = _authService.UpdateUser(request, currentUserTag);

        // Attempt to update user data
        if (userToken != "")
        {
            return Ok(new { message = "User data update successfully", user_token = userToken });
        }
        else
        {
            return BadRequest(new { message = "User data update failed" });
        }
    }

    // PATCH: api/Account/UpdatePassword
    [Authorize]
    [HttpPatch]
    public IActionResult UpdatePassword(UpdatePasswordDto request)
    {
        // Attempt to update user password
        if (_authService.UpdatePassword(request))
        {
            return Ok(new { message = "User password update successfully" });
        }
        else
        {
            return BadRequest(new { message = "User password update failed" });
        }
    }

    // DELETE: api/Account/DeleteAccount
    [Authorize]
    [HttpDelete]
    public IActionResult DeleteAccount()
    {
        var currentUserID = User.FindFirst("id")?.Value;

        // Attempt to delete the user
        if ((currentUserID != null) && _authService.DeleteUser(currentUserID))
        {
            return Ok(new { message = "Account deleted successfully" });
        }
        else
        {
            return BadRequest(new { message = "User delete failed" });
        }
    }
}