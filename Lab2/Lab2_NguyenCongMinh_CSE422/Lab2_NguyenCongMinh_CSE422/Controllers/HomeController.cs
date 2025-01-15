using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;
using Lab2_NguyenCongMinh_CSE422.Models.ViewModels;

namespace Lab2_NguyenCongMinh_CSE422.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IRepository<DeviceCategory> _categoryRepository;
    private readonly IRepository<User> _userRepository;

    public HomeController(
        ILogger<HomeController> logger,
        IDeviceRepository deviceRepository,
        IRepository<DeviceCategory> categoryRepository,
        IRepository<User> userRepository)
    {
        _logger = logger;
        _deviceRepository = deviceRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index()
    {
        var devices = await _deviceRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();
        var users = await _userRepository.GetAllAsync();

        var viewModel = new DashboardViewModel
        {
            TotalDevices = devices.Count(),
            TotalCategories = categories.Count(),
            TotalUsers = users.Count(),
            DevicesInUse = devices.Count(d => d.Status == DeviceStatus.InUse),
            DevicesBroken = devices.Count(d => d.Status == DeviceStatus.Broken),
            DevicesInMaintenance = devices.Count(d => d.Status == DeviceStatus.UnderMaintenance),
            RecentDevices = devices.OrderByDescending(d => d.DateOfEntry).Take(5).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
