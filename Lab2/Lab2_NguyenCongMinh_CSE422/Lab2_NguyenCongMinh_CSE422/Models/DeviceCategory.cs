using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    /// <summary>
    /// Represents a category of devices in the system
    /// </summary>
    public class DeviceCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the category
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of devices in this category
        /// </summary>
        public virtual ICollection<Device> Devices { get; set; }

        /// <summary>
        /// Gets the count of devices in this category
        /// </summary>
        [NotMapped]
        public int DeviceCount => Devices?.Count ?? 0;

        /// <summary>
        /// Initializes a new instance of the DeviceCategory class
        /// </summary>
        public DeviceCategory()
        {
            Devices = new HashSet<Device>();
        }
    }
}
