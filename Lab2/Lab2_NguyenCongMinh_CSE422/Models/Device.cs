using System.ComponentModel.DataAnnotations;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    /// <summary>
    /// Represents a device in the system
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Gets or sets the unique identifier for the device
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the device
        /// </summary>
        [Required(ErrorMessage = "Device name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique code for the device
        /// </summary>
        [Required(ErrorMessage = "Device code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [RegularExpression(@"^[A-Za-z0-9-_]+$", ErrorMessage = "Code can only contain letters, numbers, hyphens, and underscores")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the category ID of the device
        /// </summary>
        [Required(ErrorMessage = "Device category is required")]
        public int DeviceCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the device
        /// </summary>
        [Required(ErrorMessage = "Device status is required")]
        public DeviceStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date when the device was entered into the system
        /// </summary>
        [Required(ErrorMessage = "Date of entry is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Entry")]
        public DateTime DateOfEntry { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user currently assigned to this device
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Navigation property for the user assigned to this device
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Navigation property for the device category
        /// </summary>
        public virtual DeviceCategory Category { get; set; }
    }
}
