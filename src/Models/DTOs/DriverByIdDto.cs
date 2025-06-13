namespace Models.DTOs;

public class DriverByIdDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public int CarNumber { get; set; }
    public string CarManufacturerName { get; set; }
    public string CarModelName { get; set; }
}