using System.ComponentModel.DataAnnotations;

namespace Models;

public class Competition
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    public virtual ICollection<DriverCompetition> DriverCompetitions { get; set; } = new List<DriverCompetition>();
}