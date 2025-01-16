using System.ComponentModel.DataAnnotations;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Lab2_NguyenCongMinh_CSE422.Models.ViewModels
{
    public class DeviceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Device name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Device code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [RegularExpression(@"^[A-Za-z0-9-_]+$", ErrorMessage = "Code can only contain letters, numbers, hyphens, and underscores")]
        [Display(Name = "Device Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int DeviceCategoryId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public DeviceStatus Status { get; set; }

        [Required(ErrorMessage = "Date of entry is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Entry")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Assigned User")]
        public int? UserId { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        
        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public IEnumerable<DeviceCategory> Categories { get; set; } = new List<DeviceCategory>();
        
        [NotMapped]
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }

    public class DeviceFilterViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public DeviceStatus? Status { get; set; }
        public IEnumerable<DeviceCategory> Categories { get; set; }
    }
}
