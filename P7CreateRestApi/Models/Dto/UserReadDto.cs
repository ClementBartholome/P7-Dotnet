using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.Dto
{
    public class UserReadDto
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
        public string FullName { get; set; }
    }
}