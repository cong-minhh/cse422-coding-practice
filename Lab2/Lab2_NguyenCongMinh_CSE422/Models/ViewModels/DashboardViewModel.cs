namespace Lab2_NguyenCongMinh_CSE422.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalDevices { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public int DevicesInUse { get; set; }
        public int DevicesBroken { get; set; }
        public int DevicesInMaintenance { get; set; }
        public IEnumerable<Device> RecentDevices { get; set; }
    }
}
