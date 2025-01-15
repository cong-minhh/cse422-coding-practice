using System.ComponentModel.DataAnnotations;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;

namespace Lab2_NguyenCongMinh_CSE422.Models.ViewModels
{
    public class DeviceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Device name is required")]
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Device code is required")]
        [Display(Name = "Device Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int DeviceCategoryId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public DeviceStatus Status { get; set; }

        [Required(ErrorMessage = "Date of entry is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Entry")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Assigned User")]
        public int? UserId { get; set; }

        public string CategoryName { get; set; }
        public string UserName { get; set; }

        public IEnumerable<DeviceCategory> Categories { get; set; }
        public IEnumerable<User> Users { get; set; }
    }

    public class DeviceFilterViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public DeviceStatus? Status { get; set; }
        public IEnumerable<DeviceCategory> Categories { get; set; }
    }
}
