using System.ComponentModel.DataAnnotations;

namespace Models;

public class DriverCompetition
{
    [Required]
    public int DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    [Required]
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }
    [Required]
    public DateTime Date { get; set; }
}