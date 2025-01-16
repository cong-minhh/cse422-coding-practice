using Microsoft.AspNetCore.Mvc;
using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.ViewModels;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Lab2_NguyenCongMinh_CSE422.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<UserController> _logger;
        private readonly DeviceManagementContext _context;

        public UserController(
            IRepository<User> userRepository, 
            IDeviceRepository deviceRepository,
            ILogger<UserController> logger,
            DeviceManagementContext context)
        {
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
            _logger = logger;
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            var devices = await _deviceRepository.GetDevicesByUserAsync(id.Value);
            var viewModel = new UserDetailsViewModel
            {
                User = user,
                AssignedDevices = devices
            };

            return View(viewModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email is already in use
                var existingUser = await _userRepository.FindAsync(u => u.Email == user.Email);
                if (existingUser.Any())
                {
                    ModelState.AddModelError("Email", "This email address is already in use");
                    return View(user);
                }

                try
                {
                    await _userRepository.AddAsync(user);
                    await _userRepository.SaveChangesAsync();
                    TempData["Success"] = "User created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user: {Email}", user.Email);
                    ModelState.AddModelError("", "An error occurred while creating the user. Please try again.");
                }
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,PhoneNumber")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if email is already in use by another user
                var existingUser = await _userRepository.FindAsync(u => u.Email == user.Email && u.Id != user.Id);
                if (existingUser.Any())
                {
                    ModelState.AddModelError("Email", "This email address is already in use");
                    return View(user);
                }

                try
                {
                    _userRepository.Update(user);
                    await _userRepository.SaveChangesAsync();
                    TempData["Success"] = "User updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user: {UserId}", id);
                    if (!await UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while updating the user. Please try again.");
                    }
                }
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete attempted with null ID");
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(id.Value);
                if (user == null)
                {
                    _logger.LogWarning("User not found for deletion: {UserId}", id);
                    TempData["Error"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Get assigned devices for the confirmation view
                var assignedDevices = await _deviceRepository.GetDevicesByUserAsync(id.Value);
                ViewBag.HasAssignedDevices = assignedDevices.Any();
                ViewBag.AssignedDevices = assignedDevices;

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user for deletion: {UserId}", id);
                TempData["Error"] = "Error loading user. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _context.Users
                        .Include(u => u.Devices)
                        .Include(u => u.UserRoles)
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                    {
                        _logger.LogWarning("User not found for deletion: {UserId}", id);
                        TempData["Error"] = "User not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Store user info for logging
                    var userInfo = $"{user.FullName} ({user.Email})";

                    // Remove UserRole relationships
                    if (user.UserRoles != null && user.UserRoles.Any())
                    {
                        _context.UserRoles.RemoveRange(user.UserRoles);
                    }

                    // Unassign all devices from this user
                    foreach (var device in user.Devices.ToList())
                    {
                        device.UserId = null;
                        _context.Devices.Update(device);
                    }

                    // Remove the user
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    _logger.LogInformation("Successfully deleted user: {UserInfo}", userInfo);
                    TempData["Success"] = $"User '{userInfo}' has been deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error deleting user: {UserId}", id);
                    TempData["Error"] = "An error occurred while deleting the user. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        private async Task<bool> UserExists(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null;
        }
    }
}
