using System.ComponentModel.DataAnnotations;

public class User
{
    public int UserId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; } 
    [Required]
    public string Role { get; set; } 
}
