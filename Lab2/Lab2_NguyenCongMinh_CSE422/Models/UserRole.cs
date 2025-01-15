using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
