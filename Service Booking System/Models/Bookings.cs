using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service_Booking_System.Models;

public class Bookings
{
    [Key]
    public Guid BookingId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public Users User { get; set; }
    [Required]
    public Guid ServiceId { get; set; }
    
    [ForeignKey("ServiceId")]
    public Services Service { get; set; }
    [Required]
    public DateTime BookingDate { get; set; }
    [Required]
    public string Status { get; set; }
}