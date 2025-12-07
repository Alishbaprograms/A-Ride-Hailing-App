using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Driver
{
    public int DriverId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [Required, StringLength(100)]
    public string Vehicle { get; set; }

    public bool Availability { get; set; } = true;
}
