using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;

namespace Lab2_NguyenCongMinh_CSE422.Models.Repositories
{
    public class UserRepository : BaseRepository<User>, IRepository<User>
    {
        private readonly DeviceManagementContext _context;

        public UserRepository(DeviceManagementContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Devices)
                .ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Devices)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
                .Include(u => u.Devices)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public override void Update(User entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Users.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        public override void Remove(User entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Users.Attach(entity);
            }
            _context.Users.Remove(entity);
        }
    }
}
