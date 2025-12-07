using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

[Authorize(Roles = "User")]
public class RideController : Controller
{
    private readonly ApplicationDbContext _context;

    public RideController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Ride/BookRide
    [HttpGet]
    public IActionResult BookRide()
    {
        return View();
    }

    // POST: /Ride/BookRide
    [HttpPost]
    public IActionResult BookRide(string pickupLocation, string dropLocation)
    {
        if (string.IsNullOrWhiteSpace(pickupLocation) || string.IsNullOrWhiteSpace(dropLocation))
        {
            ViewBag.Error = "Both pickup and drop locations are required.";
            return View();
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var ride = new Ride
        {
            UserId = userId,
            PickupLocation = pickupLocation,
            DropLocation = dropLocation,
            Status = "Pending"
        };

        _context.Rides.Add(ride);
        _context.SaveChanges();

        return RedirectToAction("MyRides");
    }


    public IActionResult MyRides()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var rides = _context.Rides
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RequestedAt)
            .ToList();

        return View(rides);
    }
}
