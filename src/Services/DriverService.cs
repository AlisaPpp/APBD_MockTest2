using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Services.DbContext;

namespace Services;

public class DriverService : IDriverService
{
    private readonly DriverDbContext _context;

    public DriverService(DriverDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AllDriversDto>> GetAllDrivers(string? sortBy, CancellationToken token)
    {
        try
        {
            IQueryable<Driver> query = _context.Drivers;

            query = sortBy?.ToLower() switch
            {
                "lastname" => query.OrderBy(d => d.LastName),
                "birthday" => query.OrderBy(d => d.Birthday),
                _ => query.OrderBy(d => d.FirstName) 
            };

            var drivers = await query.ToListAsync(token);
            return drivers.Select(MapDriversToDto).ToList();
        }
        catch (Exception)
        {
            throw new ApplicationException("Error getting all drivers");
        }
    }

    
    public async Task<IEnumerable<AllCompetitionsDto>> GetAllCompetitions(int driverId, CancellationToken token)
    {
        try
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driverId, token);
            if (driver == null)
                throw new KeyNotFoundException($"Driver with id {driverId} not found");
            var competitions = await _context.DriverCompetitions
                .Where(c => c.DriverId == driverId)
                .Select(c => new AllCompetitionsDto
                {
                    Name = c.Competition.Name,
                    Date = c.Date
                })
                .ToListAsync();

            return competitions;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error getting all competitions");
        }
        

    }

    public async Task<DriverByIdDto?> GetDriverById(int driverId, CancellationToken token)
    {
        try
        {
            var driver = await _context.Drivers
                .Include(d => d.Car)
                .ThenInclude(c => c.CarManufacturer)
                .FirstOrDefaultAsync(d => d.Id == driverId, token);

            if (driver == null)
                return null;

            var driverDto = new DriverByIdDto
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Birthday = driver.Birthday,
                CarManufacturerName = driver.Car.CarManufacturer.Name,
                CarModelName = driver.Car.ModelName,
                CarNumber = driver.Car.Number
            };

            return driverDto;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error getting driver by id", ex);
        }
    }

    public async Task<bool> AddDriver(CreateDriverDto driverDto, CancellationToken token)
    {
        if (driverDto.FirstName == null)
            throw new ArgumentException("First name is required");
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == driverDto.CarId, token);
        if (car == null)
            throw new KeyNotFoundException($"Car with id {driverDto.CarId} not found");

        try
        {
            var driver = new Driver
            {
                FirstName = driverDto.FirstName,
                LastName = driverDto.LastName,
                Birthday = driverDto.Birthday,
                CarId = car.Id,
            };
            await _context.Drivers.AddAsync(driver, token);
            await _context.SaveChangesAsync(token);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error adding driver", ex);
        }
    }

    public async Task<bool> AssignDriver(AssignDriverRequestDto requestDto, CancellationToken token)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == requestDto.DriverId, token);
        if (driver == null) 
            throw new KeyNotFoundException($"Driver with id {requestDto.DriverId} not found");
        var competition = await _context.Competitions.FirstOrDefaultAsync(c => c.Id == requestDto.CompetitionId, token);
        if (competition == null)
            throw new KeyNotFoundException($"Competition with id {requestDto.CompetitionId} not found");
        try
        {
            var newDriverCompetition = new DriverCompetition
            {
                DriverId = driver.Id,
                CompetitionId = competition.Id,
            };
            await _context.DriverCompetitions.AddAsync(newDriverCompetition, token);
            await _context.SaveChangesAsync(token);
            return true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error assigning driver to competition", ex);
        }
    }

    private AllDriversDto MapDriversToDto(Driver driver)
    {
        return new AllDriversDto()
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Birthday = driver.Birthday,
        };
    }
}