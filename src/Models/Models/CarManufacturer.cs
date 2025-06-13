using System.ComponentModel.DataAnnotations;

namespace Models;

public class CarManufacturer
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    
}