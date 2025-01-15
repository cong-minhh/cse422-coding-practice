using Lab2_NguyenCongMinh_CSE422.Models.Enums;

namespace Lab2_NguyenCongMinh_CSE422.Models.Interfaces
{
    /// <summary>
    /// Interface for device-specific repository operations
    /// </summary>
    public interface IDeviceRepository : IRepository<Device>
    {
        /// <summary>
        /// Gets devices by category
        /// </summary>
        Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryId);

        /// <summary>
        /// Gets devices by status
        /// </summary>
        Task<IEnumerable<Device>> GetDevicesByStatusAsync(DeviceStatus status);

        /// <summary>
        /// Searches devices by name or code
        /// </summary>
        Task<IEnumerable<Device>> SearchDevicesAsync(string searchTerm);

        /// <summary>
        /// Gets devices assigned to a specific user
        /// </summary>
        Task<IEnumerable<Device>> GetDevicesByUserAsync(int userId);
    }
}
