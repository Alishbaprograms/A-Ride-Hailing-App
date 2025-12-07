using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

[Authorize(Roles = "Driver")]
public class DriverController : Controller
{
    private readonly ApplicationDbContext _context;

    public DriverController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userId);

        // Pending rides (not yet assigned)
        var pendingRides = _context.Rides
            .Where(r => r.Status == "Pending")
            .ToList();

        // Rides assigned to this driver
        var myRides = _context.Rides
            .Where(r => r.DriverId == driver.DriverId)
            .OrderByDescending(r => r.RequestedAt)
            .ToList();

        ViewBag.Driver = driver;
        ViewBag.PendingRides = pendingRides;

        return View(myRides);
    }

    // POST: /Driver/AcceptRide/5
    [HttpPost]
    public IActionResult AcceptRide(int rideId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userId);

        var ride = _context.Rides.FirstOrDefault(r => r.RideId == rideId);

        if (ride == null || ride.Status != "Pending")
            return NotFound();

        ride.DriverId = driver.DriverId;
        ride.Status = "Accepted";
        driver.Availability = false;

        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

    // POST: /Driver/UpdateStatus/5
    [HttpPost]
    public IActionResult UpdateStatus(int rideId, string status)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var driver = _context.Drivers.FirstOrDefault(d => d.UserId == userId);

        var ride = _context.Rides.FirstOrDefault(r => r.RideId == rideId && r.DriverId == driver.DriverId);
        if (ride == null)
            return NotFound();

        ride.Status = status;

        if (status == "Completed")
            driver.Availability = true;

        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }
}
