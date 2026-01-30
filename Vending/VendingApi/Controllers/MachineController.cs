using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendingApi.Models;
using WEMMApi.Dtos;
using VendingApi.Contexts;
using WEMMApi.Services;

namespace WEMMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController(AppDbContext context) : ControllerBase
    {
        private readonly MachineService _machineService = new(context);

        [HttpGet("Table")]
        [Authorize]
        public async Task<ActionResult<MashineResponse>> GetMachineDtosAsync([FromQuery] int currentPage, [FromQuery] int pageSize, [FromQuery] string? filter)
        {
            var result = _machineService.GetMachineDtosQueryByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (filter is not null)
                result = _machineService.ApplyFilter(result, filter);

            var totalCount = result.Count();

            result = _machineService.ApplyPagination(result, currentPage, pageSize);

            var response = new MashineResponse()
            {
                TotalCount = totalCount,
                MachineDtos = await result.ToListAsync()
            };

            return response;
        }

        [HttpGet("Dependencies")]
        [Authorize]
        public async Task<ActionResult<DependencesDto>> GetDependencesAsync()
        {
            var result = new DependencesDto();

            result.ServicePriorities = await context.ServicePriorities.ToListAsync();
            result.Users = await context.Users.Select(u => new UserListDto() 
            {
                UserId = u.UserId,
                LastName = u.LastName
            }).ToListAsync();
            result.WorkModes = await context.WorkModes.ToListAsync();
            result.Templates = await context.Templates.ToListAsync();
            result.Models = await context.Models.ToListAsync();
            result.MachinePlaces = await context.MachinePlaces.ToListAsync();
            result.Managers = await context.Workers.Include(w => w.User).Select(w => new WorkerListDto()
            {
                WorkerId = w.WorkerId,
                UserId = w.UserId,
                LastName = w.User.LastName,
                IsEngineer = w.IsEngineer,
                IsManager = w.IsManager,
                IsTechnician = w.IsTechnician
            }).Where(w => w.IsManager).ToListAsync();
            result.Enginers = await context.Workers.Include(w => w.User).Select(w => new WorkerListDto()
            {
                WorkerId = w.WorkerId,
                UserId = w.UserId,
                LastName = w.User.LastName,
                IsEngineer = w.IsEngineer,
                IsManager = w.IsManager,
                IsTechnician = w.IsTechnician
            }).Where(w => w.IsEngineer).ToListAsync();
            result.Technics = await context.Workers.Include(w => w.User).Select(w => new WorkerListDto()
            {
                WorkerId = w.WorkerId,
                UserId = w.UserId,
                LastName = w.User.LastName,
                IsEngineer = w.IsEngineer,
                IsManager = w.IsManager,
                IsTechnician = w.IsTechnician
            }).Where(w => w.IsTechnician).ToListAsync();

            return result;
        }

        [HttpPost("Machine")]
        [Authorize]
        public async Task<IActionResult> PostMachineAsync([FromBody] VendingMachine machine)
        {
            await _machineService.CreateMachine(machine, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created();
        }
    }
}
