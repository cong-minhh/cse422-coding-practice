using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.ViewModels;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Lab2_NguyenCongMinh_CSE422.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRepository<DeviceCategory> _categoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(
            IDeviceRepository deviceRepository,
            IRepository<DeviceCategory> categoryRepository,
            IRepository<User> userRepository,
            ILogger<DeviceController> logger)
        {
            _deviceRepository = deviceRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: Device
        public async Task<IActionResult> Index(DeviceFilterViewModel filter)
        {
            var devices = await _deviceRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                devices = await _deviceRepository.SearchDevicesAsync(filter.SearchTerm);
            }

            if (filter.CategoryId.HasValue)
            {
                devices = await _deviceRepository.GetDevicesByCategoryAsync(filter.CategoryId.Value);
            }

            if (filter.Status.HasValue)
            {
                devices = await _deviceRepository.GetDevicesByStatusAsync(filter.Status.Value);
            }

            var viewModel = new DeviceFilterViewModel
            {
                Categories = categories,
                SearchTerm = filter.SearchTerm,
                CategoryId = filter.CategoryId,
                Status = filter.Status
            };

            ViewBag.Devices = devices;
            return View(viewModel);
        }

        // GET: Device/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Device/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var users = await _userRepository.GetAllAsync();

                var viewModel = new DeviceViewModel
                {
                    Categories = categories,
                    Users = users,
                    DateOfEntry = DateTime.Now
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GET Create action");
                TempData["Error"] = "Error loading create form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceViewModel viewModel)
        {
            try
            {
                _logger.LogInformation("Attempting to create device: {DeviceName}", viewModel.Name);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for device creation");
                    // Remove validation errors for NotMapped properties
                    ModelState.Remove("CategoryName");
                    ModelState.Remove("UserName");
                    ModelState.Remove("Categories");
                    ModelState.Remove("Users");

                    if (!ModelState.IsValid)
                    {
                        viewModel.Categories = await _categoryRepository.GetAllAsync();
                        viewModel.Users = await _userRepository.GetAllAsync();
                        return View(viewModel);
                    }
                }

                var device = new Device
                {
                    Name = viewModel.Name,
                    Code = viewModel.Code,
                    DeviceCategoryId = viewModel.DeviceCategoryId,
                    Status = viewModel.Status,
                    DateOfEntry = viewModel.DateOfEntry,
                    UserId = viewModel.UserId
                };

                await _deviceRepository.AddAsync(device);
                await _deviceRepository.SaveChangesAsync();

                _logger.LogInformation("Successfully created device: {DeviceName} with ID: {DeviceId}", device.Name, device.Id);
                TempData["Success"] = "Device created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating device: {DeviceName}", viewModel.Name);
                ModelState.AddModelError("", "Error creating device. Please try again.");
                viewModel.Categories = await _categoryRepository.GetAllAsync();
                viewModel.Users = await _userRepository.GetAllAsync();
                return View(viewModel);
            }
        }

        // GET: Device/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                _logger.LogInformation("Loading device for edit: {DeviceId}", id);
                var device = await _deviceRepository.GetByIdAsync(id);
                if (device == null)
                {
                    _logger.LogWarning("Device not found for edit: {DeviceId}", id);
                    TempData["Error"] = "Device not found.";
                    return RedirectToAction(nameof(Index));
                }

                var categories = await _categoryRepository.GetAllAsync();
                var users = await _userRepository.GetAllAsync();

                var viewModel = new DeviceViewModel
                {
                    Id = device.Id,
                    Name = device.Name,
                    Code = device.Code,
                    DeviceCategoryId = device.DeviceCategoryId,
                    Status = device.Status,
                    DateOfEntry = device.DateOfEntry,
                    UserId = device.UserId,
                    Categories = categories,
                    Users = users,
                    CategoryName = device.Category?.Name,
                    UserName = device.User?.FullName
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading device for edit: {DeviceId}", id);
                TempData["Error"] = "Error loading device. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Device/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceViewModel viewModel)
        {
            try
            {
                _logger.LogInformation("Attempting to update device: {DeviceId}", id);

                if (id != viewModel.Id)
                {
                    _logger.LogWarning("Invalid device ID for edit. Expected: {ExpectedId}, Actual: {ActualId}", id, viewModel.Id);
                    TempData["Error"] = "Invalid device ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for device update");
                    // Remove validation errors for NotMapped properties
                    ModelState.Remove("CategoryName");
                    ModelState.Remove("UserName");
                    ModelState.Remove("Categories");
                    ModelState.Remove("Users");

                    if (!ModelState.IsValid)
                    {
                        viewModel.Categories = await _categoryRepository.GetAllAsync();
                        viewModel.Users = await _userRepository.GetAllAsync();
                        return View(viewModel);
                    }
                }

                var device = await _deviceRepository.GetByIdAsync(id);
                if (device == null)
                {
                    _logger.LogWarning("Device not found for update: {DeviceId}", id);
                    TempData["Error"] = "Device not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Update device properties
                device.Name = viewModel.Name;
                device.Code = viewModel.Code;
                device.DeviceCategoryId = viewModel.DeviceCategoryId;
                device.Status = viewModel.Status;
                device.DateOfEntry = viewModel.DateOfEntry;
                device.UserId = viewModel.UserId;

                _deviceRepository.Update(device);
                await _deviceRepository.SaveChangesAsync();

                _logger.LogInformation("Successfully updated device: {DeviceId}", id);
                TempData["Success"] = "Device updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating device: {DeviceId}", id);
                ModelState.AddModelError("", "Error updating device. Please try again.");
                viewModel.Categories = await _categoryRepository.GetAllAsync();
                viewModel.Users = await _userRepository.GetAllAsync();
                return View(viewModel);
            }
        }

        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    _logger.LogWarning("Delete attempted with null ID");
                    TempData["Error"] = "Invalid device ID.";
                    return RedirectToAction(nameof(Index));
                }

                var device = await _deviceRepository.GetByIdAsync(id.Value);
                if (device == null)
                {
                    _logger.LogWarning("Device not found for deletion: {DeviceId}", id);
                    TempData["Error"] = "Device not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new DeviceViewModel
                {
                    Id = device.Id,
                    Name = device.Name,
                    Code = device.Code,
                    DeviceCategoryId = device.DeviceCategoryId,
                    Status = device.Status,
                    DateOfEntry = device.DateOfEntry,
                    UserId = device.UserId,
                    CategoryName = device.Category?.Name,
                    UserName = device.User?.FullName
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading device for deletion: {DeviceId}", id);
                TempData["Error"] = "Error loading device. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete device: {DeviceId}", id);

                var device = await _deviceRepository.GetByIdAsync(id);
                if (device == null)
                {
                    _logger.LogWarning("Device not found for deletion: {DeviceId}", id);
                    TempData["Error"] = "Device not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Store device info for logging
                var deviceInfo = $"{device.Name} ({device.Code})";

                // Remove the device
                _deviceRepository.Remove(device);
                await _deviceRepository.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted device: {DeviceInfo}", deviceInfo);
                TempData["Success"] = $"Device '{deviceInfo}' has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting device: {DeviceId}", id);
                TempData["Error"] = "An error occurred while deleting the device. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
