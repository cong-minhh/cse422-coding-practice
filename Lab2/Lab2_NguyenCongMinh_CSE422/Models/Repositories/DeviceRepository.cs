using Microsoft.EntityFrameworkCore;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;

namespace Lab2_NguyenCongMinh_CSE422.Models.Repositories
{
    /// <summary>
    /// Implementation of device-specific repository operations
    /// </summary>
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(DeviceManagementContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.DeviceCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByStatusAsync(DeviceStatus status)
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> SearchDevicesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.Name.ToLower().Contains(searchTerm) || 
                           d.Code.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByUserAsync(int userId)
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public override async Task<Device> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Category)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
