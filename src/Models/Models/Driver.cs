using System.ComponentModel.DataAnnotations;

namespace Models;

public class Driver
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(200)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(200)]
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    [Required]
    public int CarId { get; set; }
    public virtual Car? Car { get; set; }
    public virtual ICollection<DriverCompetition> DriverCompetitions { get; set; } = new List<DriverCompetition>();

}