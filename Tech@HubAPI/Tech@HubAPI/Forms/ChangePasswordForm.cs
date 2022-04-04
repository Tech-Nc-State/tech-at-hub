using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Tech_HubAPI.Services;
using Tech_HubAPI.Models;

public class ChangePasswordForm
{
    public ChangePasswordForm(string userName, string oldPassword, string newPassword, string newPasswordRetyped)
    {
        this.UserName = userName;
        this.OldPassword = oldPassword;
        this.NewPassword = newPassword;
        this.NewPasswordRetyped = newPasswordRetyped;
    }

    public string UserName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordRetyped { get; set; }

    public void Validate()
    {
        if (NewPassword.CompareTo(NewPasswordRetyped) != 0)
        {
            throw new ArgumentException("The retyped password does not match the initial new password!");
        }

    }





}
