using System.ComponentModel.DataAnnotations;

namespace Service_Booking_System.Models;

public class Services
{
    [Key]
    public Guid ServiceId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
}