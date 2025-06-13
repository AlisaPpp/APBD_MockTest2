namespace Models.DTOs;

public class CreateDriverDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public int CarId { get; set; }
}