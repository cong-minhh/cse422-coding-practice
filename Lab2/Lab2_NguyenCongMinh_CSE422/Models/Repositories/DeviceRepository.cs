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
        private readonly DeviceManagementContext _deviceContext;

        public DeviceRepository(DeviceManagementContext context) : base(context)
        {
            _deviceContext = context;
        }

        public override async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _deviceContext.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByCategoryAsync(int categoryId)
        {
            return await _deviceContext.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.DeviceCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByStatusAsync(DeviceStatus status)
        {
            return await _deviceContext.Devices
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
            return await _deviceContext.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.Name.ToLower().Contains(searchTerm) || 
                           d.Code.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByUserAsync(int userId)
        {
            return await _deviceContext.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public override async Task<Device> GetByIdAsync(int id)
        {
            return await _deviceContext.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public override async Task AddAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            await _deviceContext.Devices.AddAsync(device);
        }

        public override void Update(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            _deviceContext.Entry(device).State = EntityState.Modified;
        }

        public override void Remove(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            _deviceContext.Devices.Remove(device);
        }

        public override async Task SaveChangesAsync()
        {
            await _deviceContext.SaveChangesAsync();
        }
    }
}
