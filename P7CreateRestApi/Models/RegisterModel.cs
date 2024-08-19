namespace P7CreateRestApi.Models;

public class RegisterModel
{
    public RegisterModel(string userName, string fullName, string email, string password)
    {
        UserName = userName;
        FullName = fullName;
        Email = email;
        Password = password;
    }

    public string UserName  { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}