using Microsoft.AspNetCore.Mvc;
using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.ViewModels;
using System.Linq.Expressions;

namespace Lab2_NguyenCongMinh_CSE422.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IDeviceRepository _deviceRepository;

        public UserController(IRepository<User> userRepository, IDeviceRepository deviceRepository)
        {
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
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
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                try
                {
                    _userRepository.Remove(user);
                    await _userRepository.SaveChangesAsync();
                    TempData["Success"] = "User deleted successfully";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while deleting the user. Please try again.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null;
        }
    }
}
