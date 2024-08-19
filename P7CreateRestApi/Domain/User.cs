using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Domain
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}