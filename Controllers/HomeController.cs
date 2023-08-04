using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dvd_rental_app.Models;
using dvd_rental_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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


    [Route("/")]
    [Route("Home/")]
    [Route("Home/Index")]
    public IActionResult Index()
    {
        _logger.LogInformation("Home page retrieved and loading...");
        return View();
    }


    [Route("Home/Movies")]
    public async Task<IActionResult> Movies()
    {
        if (_db.Movie == null)
        {
            _logger.LogError("Database data for movies not found. Pelase check and try again!");
            return NotFound();
        }

        var MovieList = await _db.Movie.ToListAsync();

        _logger.LogInformation("Movie list retrieved successfully.");
        return View(MovieList);
    }


    [Authorize]
    [Route("Home/Movie/Details/{id}")]
    public async Task<IActionResult> MovieDetails(int id)
    {
        var movie = await _db.Movie.Where(m => m.MovieId == id).FirstOrDefaultAsync();

        if (movie == null)
        {
            _logger.LogError("Could not find a movie with the specified ID. Pelase check and try again!");
        }

        _logger.LogInformation("Movie list retrieved successfully.");
        return View(movie);
    }


    [Authorize]
    [Route("Home/MoviesByGenre/{genre}")]
    public async Task<IActionResult> MoviesByGenre(string genre)
    {
        var movies = await _db.Movie.Where(m => m.Genre == genre).ToListAsync();

        if (movies == null)
        {
            _logger.LogError("Could not find any movie with the specified genre. Pelase check and try again!");
        }

        ViewBag.Genre = genre;

        _logger.LogInformation("Movie(s) with specified genre retrieved successfully.");
        return View(movies);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

