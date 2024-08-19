﻿namespace P7CreateRestApi.Models;

public class RegisterModel
{
    public RegisterModel(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        Password = password;
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}