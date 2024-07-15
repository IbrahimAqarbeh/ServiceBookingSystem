using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Service_Booking_System.Models;

public class Users
{
    [Key]
    public Guid UserId { get; set; }
    [Required]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Password { get; set; }
    
    [JsonIgnore]
    public string Discriminator { get; set; } = "User";
}