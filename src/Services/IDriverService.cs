using Models;
using Models.DTOs;

namespace Services;

public interface IDriverService
{
    public Task<IEnumerable<AllDriversDto>> GetAllDrivers(string? sortBy, CancellationToken token);
    public Task<DriverByIdDto?> GetDriverById(int id, CancellationToken token);
    public Task<bool> AddDriver(CreateDriverDto driverDto, CancellationToken token);
    public Task<bool> AssignDriver(AssignDriverRequestDto requestDto, CancellationToken token);
    public Task<IEnumerable<AllCompetitionsDto>> GetAllCompetitions(int driverId, CancellationToken token);
}