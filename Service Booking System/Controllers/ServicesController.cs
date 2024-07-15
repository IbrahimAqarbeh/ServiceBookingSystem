using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Booking_System.Models;

namespace Service_Booking_System.Controllers;

public class ServicesController : Controller
{
    private readonly ApplicationDbContext _context; 
    
    public ServicesController (ApplicationDbContext context) 
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    [Route("api/[controller]/services")]
    public IActionResult Services()
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null)
        {
            return Ok(_context.Services.ToList());
        }

        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpGet]
    [Route("api/[controller]/{id}")]
    public IActionResult GetService(Guid id)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null)
        {
            Services service = _context.Services.Where(u => u.ServiceId.Equals(id)).FirstOrDefault();
            if (service != null)
            {
                return Ok(service);
            }

            return NotFound("Service wasn't found, please check the ID");
        }

        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpPost]
    [Route("api/[controller]/services")]
    public IActionResult CreateService([FromBody] Services service)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null && myUser.Discriminator.Equals("Administrator"))
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return Ok("Service was saved successfully");
        }
        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpPut]
    [Route("api/[controller]/{id}")]
    public IActionResult UpdateService(Guid id, [FromBody] Services UpdatedService)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null && myUser.Discriminator.Equals("Administrator"))
        {
            Services service = _context.Services.FirstOrDefault(u => u.ServiceId.Equals(id));
            if (service != null)
            {
                service.Description = UpdatedService.Description;
                service.Name = UpdatedService.Name;
                service.Price = UpdatedService.Price;
                _context.SaveChanges();
                return Ok("Service was updated successfully");
            }

            return NotFound("Service wasn't found");
        }

        return Unauthorized("User isn't authorized");
    }
    [Authorize]
    [HttpDelete]
    [Route("api/[controller]/{id}")]
    public IActionResult DeleteService(Guid id)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var myUser = _context.Users.FirstOrDefault(u => u.Email!.Equals(userEmail));
        if (myUser != null && myUser.Discriminator.Equals("Administrator"))
        {
            Services service = _context.Services.FirstOrDefault(u => u.ServiceId.Equals(id));
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
                return Ok("Service was deleted successfully");
            }

            return NotFound("Service wasn't found, please check the ID");
        }

        return Unauthorized("User isn't authorized");
    }
}