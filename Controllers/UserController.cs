using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /User/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /User/Register
    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("", "Email already exists.");
            return View(model);
        }

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            PasswordHash = model.Password, // for assignment only; use hashing in real life
            Role = model.Role
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // If role is Driver, also create Driver entry
        if (model.Role == "Driver")
        {
            var driver = new Driver
            {
                UserId = user.UserId,
                Vehicle = "Unknown",
                Availability = true
            };
            _context.Drivers.Add(driver);
            _context.SaveChanges();
        }

        return RedirectToAction("Login");
    }

    // GET: /User/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /User/Login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users
            .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login.");
            return View(model);
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Redirect based on role
        if (user.Role == "Driver")
            return RedirectToAction("Dashboard", "Driver");

        return RedirectToAction("BookRide", "Ride");
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
