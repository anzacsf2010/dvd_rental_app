using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dvd_rental_app.Models;
using dvd_rental_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NewRelic.Api.Agent;
using Microsoft.AspNetCore.Identity;
using dvd_rental_app.Services;

namespace dvd_rental_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICartService _cartService;


    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager, ICartService cartService)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
        _cartService = cartService;
    }


    public DbSet<Movie> Movie { get; set; } = default!;


#pragma warning disable CS8600, CS8602 // Dereference of a possibly null reference.
    [Trace]
    [Route("/")]
    [Route("Home/")]
    [Route("Home/Index")]
    public IActionResult Index()
    {
        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.AddCustomAttribute("CustomHomeAttribute", "This is the homepage custom attribute");

        _logger.LogInformation("Home page retrieved and loading...");
        return View();
    }


    [Trace]
    [Authorize]
    [Route("Home/Movies")]
    public async Task<IActionResult> Movies()
    {
        if (_db.Movie == null)
        {
            _logger.LogError("Database data for movies not found. Pelase check and try again!");
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogError("User not logged in. Please sign in and try again!");
            return NotFound();
        }

        string userId = user.Id;
        string userName = user.UserName;

        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.SetUserId(userId);
        transaction.AddCustomAttribute("TransactionStart", DateTime.Now);
        transaction.AddCustomAttribute("UserID", userId.ToString());
        transaction.AddCustomAttribute("UserName", userName.ToString());

        var MovieList = await _db.Movie.ToListAsync();

        transaction.AddCustomAttribute("TransactionEnd", DateTime.Now);
        _logger.LogInformation("Movie list retrieved successfully.");
        return View(MovieList);
    }


    [Trace]
    [Authorize]
    [Route("Home/Movie/Details/{id}")]
    public async Task<IActionResult> MovieDetails(int id)
    {
        var movie = await _db.Movie.Where(m => m.MovieId == id).FirstOrDefaultAsync();

        if (movie == null)
        {
            _logger.LogError("Could not find a movie with the specified ID. Pelase check and try again!");
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogError("User not logged in. Please sign in and try again!");
            return NotFound();
        }

        string userId = user.Id;
        string userName = user.UserName;

        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.SetUserId(userId);
        transaction.AddCustomAttribute("TransactionStart", DateTime.Now);
        transaction.AddCustomAttribute("UserID", userId.ToString());
        transaction.AddCustomAttribute("UserName", userName.ToString());
        transaction.AddCustomAttribute("MovieId", movie.MovieId.ToString());
        transaction.AddCustomAttribute("MovieName", movie.Title.ToString());

        _logger.LogInformation("Movie list retrieved successfully.");
        transaction.AddCustomAttribute("TransactionEnd", DateTime.Now);
        return View(movie);
    }


    [Trace]
    [Authorize]
    [Route("Home/MoviesByGenre/{genre}")]
    public async Task<IActionResult> MoviesByGenre(string genre)
    {
        var movies = await _db.Movie.Where(m => m.Genre == genre).ToListAsync();

        if (movies == null)
        {
            _logger.LogError("Could not find any movie with the specified genre. Pelase check and try again!");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogError("User not logged in. Please sign in and try again!");
            return NotFound();
        }

        string userId = user.Id;
        string userName = user.UserName;

        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.SetUserId(userId);
        transaction.AddCustomAttribute("TransactionStart", DateTime.Now);
        transaction.AddCustomAttribute("UserID", userId.ToString());
        transaction.AddCustomAttribute("UserName", userName.ToString());
        transaction.AddCustomAttribute("MovieGenre", genre);

        ViewBag.Genre = genre;

        _logger.LogInformation("Movie(s) with specified genre retrieved successfully.");
        transaction.AddCustomAttribute("TransactionEnd", DateTime.Now);
        return View(movies);
    }


    [Trace]
    [Authorize]
    [Route("Home/Cart/")]
    public async Task<IActionResult> ViewCart()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            _logger.LogError("User not logged in. Please sign in and try again!");
            return NotFound();
        }

        var cartItems = _cartService.GetCartItems().Where(c => c.UserId == user.Id.ToString()).ToList();

        if (cartItems == null)
        {
            _logger.LogError("No items found in cart");
            return RedirectToAction("Movies");
        }

        string userId = user.Id;
        string userName = user.UserName;

        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.SetUserId(userId);
        transaction.AddCustomAttribute("TransactionStart", DateTime.Now);
        transaction.AddCustomAttribute("UserID", userId.ToString());
        transaction.AddCustomAttribute("UserName", userName.ToString());
        foreach (var movie in cartItems)
        {
            transaction.AddCustomAttribute("CartItem" + movie.MovieId.ToString(), movie.Title.ToString());
        }

        ViewData["ItemsInCart"] = cartItems.Count;

        _logger.LogInformation("Movies were found in cart and displayed successfully.");
        transaction.AddCustomAttribute("TransactionEnd", DateTime.Now);
        return View(cartItems);
    }


    [Trace]
    [Authorize]
    [Route("/Home/UpdateCart/{id}")]
    public async Task<IActionResult> UpdateCart(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            _logger.LogError("User not logged in. Please sign in and try again!");
            return NotFound();
        }

        var movieCheck = await _db.Movie.Where(m => m.MovieId == id).FirstOrDefaultAsync(m => m.MovieId == id);

        if (movieCheck == null)
        {
            _logger.LogError("Could not find a movie with the specified ID. Please check and try again!");
            return NotFound();
        }

        string userId = user.Id;
        string userName = user.UserName;

        var cartItem = new Models.CartItem
        {
            MovieId = id,
            Title = movieCheck.Title,
            AddedToCartDate = DateOnly.FromDateTime(DateTime.Now),
            UserId = user.Id.ToString()
        };

        _cartService.Update(cartItem);

        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        ITransaction transaction = agent.CurrentTransaction;
        transaction.SetUserId(userId);
        transaction.AddCustomAttribute("TransactionStart", DateTime.Now);
        transaction.AddCustomAttribute("UserID", userId.ToString());
        transaction.AddCustomAttribute("UserName", userName.ToString());
        transaction.AddCustomAttribute("MovieAddedOrRemovedFromCart", movieCheck.Title.ToString());

        _logger.LogInformation("The selected movie was added to or removed from cart successfully.");
        transaction.AddCustomAttribute("TransactionEnd", DateTime.Now);
        return RedirectToAction("Movies");
    }


#pragma warning restore CS8600, CS8602 // Dereference of a possibly null reference.

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

