using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ride
{
    public int RideId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public int? DriverId { get; set; }

    [ForeignKey("DriverId")]
    public Driver Driver { get; set; }

    [Required, StringLength(200)]
    public string PickupLocation { get; set; }

    [Required, StringLength(200)]
    public string DropLocation { get; set; }

    [Required, StringLength(50)]
    public string Status { get; set; } 

    public DateTime RequestedAt { get; set; } = DateTime.Now;
}
