using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DriverController : ControllerBase{
    private readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpGet]
    [Route("/api/drivers")]
    public async Task<IResult> GetAllDrivers([FromQuery] string? sortBy, CancellationToken token)
    {
        try
        {
            var drivers = await _driverService.GetAllDrivers(sortBy, token);
            return Results.Ok(drivers);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("/api/drivers/{id}")]
    public async Task<IResult> GetDriverById(int id, CancellationToken token)
    {
        try
        {
            var driver = await _driverService.GetDriverById(id, token);
            if (driver == null)
                return Results.NotFound($"Driver with id {id} not found");
            return Results.Ok(driver);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("/api/drivers")]
    public async Task<IResult> AddDriver(CreateDriverDto driverDto, CancellationToken token)
    {
        try
        {
            await _driverService.AddDriver(driverDto, token);
            return Results.Created();
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("/api/driver/competitions")]
    public async Task<IResult> AssignDriver(AssignDriverRequestDto requestDto, CancellationToken token)
    {
        try
        {
            await _driverService.AssignDriver(requestDto, token);
            return Results.Created();
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("/api/driver/{id}/competitions")]
    public async Task<IResult> GetDriverCompetitions(int id, CancellationToken token)
    {
        try
        {
            var competitions = await _driverService.GetAllCompetitions(id, token);
            return Results.Ok(competitions);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}