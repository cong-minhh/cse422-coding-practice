using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.ViewModels;

namespace Lab2_NguyenCongMinh_CSE422.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRepository<DeviceCategory> _categoryRepository;
        private readonly IRepository<User> _userRepository;

        public DeviceController(
            IDeviceRepository deviceRepository,
            IRepository<DeviceCategory> categoryRepository,
            IRepository<User> userRepository)
        {
            _deviceRepository = deviceRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
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

        // POST: Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = await _categoryRepository.GetAllAsync();
            viewModel.Users = await _userRepository.GetAllAsync();
            return View(viewModel);
        }

        // GET: Device/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound();
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
                Categories = await _categoryRepository.GetAllAsync(),
                Users = await _userRepository.GetAllAsync()
            };

            return View(viewModel);
        }

        // POST: Device/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var device = await _deviceRepository.GetByIdAsync(id);
                if (device == null)
                {
                    return NotFound();
                }

                device.Name = viewModel.Name;
                device.Code = viewModel.Code;
                device.DeviceCategoryId = viewModel.DeviceCategoryId;
                device.Status = viewModel.Status;
                device.DateOfEntry = viewModel.DateOfEntry;
                device.UserId = viewModel.UserId;

                _deviceRepository.Update(device);
                await _deviceRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = await _categoryRepository.GetAllAsync();
            viewModel.Users = await _userRepository.GetAllAsync();
            return View(viewModel);
        }

        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _deviceRepository.Remove(device);
            await _deviceRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
