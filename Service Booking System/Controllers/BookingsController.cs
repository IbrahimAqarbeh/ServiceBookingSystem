using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Booking_System.Models;

namespace Service_Booking_System.Controllers;

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context; 
    
    public BookingsController (ApplicationDbContext context) 
    {
        _context = context;
    }
    
    [Authorize]
    [HttpPost]
    [Route("api/[controller]/bookings")]
    public IActionResult CreateBooking([FromBody] Bookings booking)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Ok("Booking was saved successfully");
        }
        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpGet]
    [Route("api/[controller]/user/{userId}")]
    public IActionResult GetUserBooking(Guid userId)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null)
        {
            var bookings = _context.Bookings.Where(u => u.UserId.Equals(userId)).ToList();
            return Ok(bookings);
        }
        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpPut]
    [Route("api/[controller]/{id}")]
    public IActionResult UpdateBookingStatus(Guid id, string status)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null)
        {
            var booking = _context.Bookings.FirstOrDefault(u => u.BookingId.Equals(id));
            if (booking != null)
            {
                booking.Status = status;
                _context.SaveChanges();
                return Ok("Booking's status has been changed successfully");
            }

            return NotFound("Booking wasn't found, please check the ID");
        }
        return Unauthorized("User isn't authorized");
    }
}