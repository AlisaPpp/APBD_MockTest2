using System.ComponentModel.DataAnnotations;

namespace Models;

public class Car
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CarManufacturerId { get; set; }
    public virtual CarManufacturer CarManufacturer { get; set; } = null!;
    [StringLength(200)]
    public string ModelName { get; set; }
    public int Number { get; set; }
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}