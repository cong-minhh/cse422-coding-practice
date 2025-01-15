namespace Lab2_NguyenCongMinh_CSE422.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public IEnumerable<Device> AssignedDevices { get; set; }
    }
}
