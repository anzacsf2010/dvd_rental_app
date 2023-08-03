using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dvd_rental_app.Models;
using dvd_rental_app.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace dvd_rental_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public DbSet<Movie> Movie { get; set; } = default!;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Movies()
    {
        var MovieList = _db.Movie.ToList();

        return View(MovieList);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

