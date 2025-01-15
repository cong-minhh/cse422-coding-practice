using System.ComponentModel.DataAnnotations;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
