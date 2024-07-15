using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service_Booking_System.Models;

namespace Service_Booking_System.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context; 
    private readonly IConfiguration _configuration; 
    public UsersController(ApplicationDbContext context, IConfiguration configuration) 
    {
        _context = context;
        _configuration = configuration;
    }
    [HttpPost]
    [Route("api/[controller]/register")]
    public IActionResult Register([FromBody] Users user)
    {
        if (ModelState.IsValid)
        {
            user.DateOfBirth = DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc);
            user.Password = SecurityUtils.HashPassword(user.Password);
            _context.Add(user);
            _context.SaveChanges();
            var jwtSecret = _configuration["Jwt:Secret"];
            var token = SecurityUtils.GenerateJwtToken(user.Email!, jwtSecret);

            return Ok(new { Token = token });
        }

        return BadRequest("Invalid Data");
    }
    [HttpPut]
    [Route("api/[controller]/usertoadmin/{id}")]
    public IActionResult UserToAdmin(Guid id)
    {
        var user = _context.Users.Where(u => u.UserId.Equals(id)).FirstOrDefault();
        if (user != null)
        {
            user.Discriminator = "Administrator";
            _context.SaveChanges();
            return Ok("User has been switched to Administrator successfully");
        }

        return NotFound("User wasn't found, please check the ID");
    }
    [HttpPost]
    [Route("api/[controller]/login")]
    public IActionResult LogIn([FromBody] CommonTypes.SignInRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email!.ToLower().Equals(request.Email.ToLower()) && u.Password!.Equals(SecurityUtils.HashPassword(request.Password)));

        if (user != null)
        {
                var jwtSecret = _configuration["Jwt:Secret"];
                var token = SecurityUtils.GenerateJwtToken(user.Email!, jwtSecret);

                return Ok(new { Token = token });
        }
        else
        {
            return Unauthorized("Invalid email or password");
        }
    }
}

