using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendingApi.Models;
using WEMMApi.Dtos;
using VendingApi.Contexts;
using WEMMApi.Services;
using VendingApi.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace WEMMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        private readonly MachineService _machineService = new(context);

        [HttpGet("Table")]
        [Authorize]
        public async Task<ActionResult<MashineResponse>> GetMachineDtosTableAsync([FromQuery] int currentPage, [FromQuery] int pageSize, [FromQuery] string? filter)
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

        [HttpPost("Monitor")]
        [Authorize]
        public async Task<ActionResult<MonitorResponce>> GetMachineDtosMonitorAsync([FromBody] MonitorRequest request)
        {
            var result = await _context.VendingMachines.Include(m => m.Model)
                .Include(m => m.StatusNavigation)
                .Where(m => m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToListAsync();

            var response = new MonitorResponce();
            foreach (var machine in result)
            {
                response.TotalContributedMoney += machine.ContributedMoney;
                response.TotalChange += machine.CoinsChange + machine.BillsChange;
            }

            response.TotalBroken = result.Where(m => m.StatusNavigation.Name == "Сломан").Count();
            response.TotalServing = result.Where(m => m.StatusNavigation.Name == "Обслуживается").Count();
            response.TotalWorking = result.Where(m => m.StatusNavigation.Name == "Работает").Count();

            var filteredResult = new List<VendingMachine>();

            if (request.GeneralFilters is not null)
                foreach (var filter in request.GeneralFilters)
                    filteredResult.AddRange(result.Where(m => m.StatusNavigation.Name == filter));

            var offerFilters = request.OfferFilters;

            var sales = await _context.Sales.ToListAsync();

            if (offerFilters is not null || offerFilters?.Count != 0)
            {
                if (offerFilters.Any(f => f == "isDisconected"))
                    filteredResult = filteredResult.Where(m => !m.IsConnection).ToList();
                if (offerFilters.Any(f => f == "HasHardwareProblems"))
                    filteredResult = filteredResult.Where(m => m.HasHardwareProblems).ToList();
                if (offerFilters.Any(f => f == "isNoSales"))
                    filteredResult = filteredResult.Where(m => sales.Any(s => s.MachineId == m.MachineId)).ToList();
                if (offerFilters.Any(f => f == "isNoEncashment"))
                    filteredResult = filteredResult.Where(m => !m.IsEncanced).ToList();
                if (offerFilters.Any(f => f == "isNoService"))
                    filteredResult = filteredResult.Where(m => !m.IsServed).ToList();
                if (offerFilters.Any(f => f == "isNoFillup"))
                    filteredResult = filteredResult.Where(m => !m.IsFilledUp).ToList();
                if (offerFilters.Any(f => f == "isFewChange"))
                    filteredResult = filteredResult.Where(m => (m.CoinsChange + m.BillsChange) < 200).ToList();
                if (offerFilters.Any(f => f == "isFewGoods"))
                    filteredResult = filteredResult.Where(m => m.TotallGoods < 40).ToList();
            }

            var machines = filteredResult.Select(m => new MachineMonitorDto()
            {
                MachineId = m.MachineId,
                SerialNumber = m.SerialNumber,
                Name = m.Name,
                Location = m.Location,
                UserId = m.UserId,
                ModelName = m.Model.Name,
                LastMaintenanceDate = m.LastMaintenanceDate,
                ContributedMoney = m.ContributedMoney,
                CoinsChange = m.CoinsChange,
                BillsChange = m.BillsChange,
                StatusName = m.StatusNavigation.Name,
                LastSale = 0
            }).ToList();

            foreach (var machine in machines)
            {
                var machineSales = sales.Where(s => s.MachineId == machine.MachineId);
                if (machineSales is null)
                    continue;
                machine.LastSale = (DateTime.Now - machineSales.Max(s => s.Timestamp)).Days;
            }

            response.Machines = machines;

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

            return result;
        }

        [HttpPost("Machine")]
        [Authorize]
        public async Task<IActionResult> PostMachineAsync([FromBody] VendingMachine machine)
        {
            if(await _machineService.CreateMachine(machine, User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Created();
            return BadRequest();
        }

        [HttpGet("News")]
        [Authorize]
        public async Task<ActionResult<List<News>>> GetNewsAsync()
        {
            return await _machineService.GetNewsByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("Efficiency")]
        [Authorize]
        public async Task<ActionResult<EfficiencyResponceDto>> GetEfficiencyAsync()
        {
            return await _machineService.GetMashinesEfficiencyAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("Sales/Today")]
        [Authorize]
        public async Task<ActionResult<SalesDto>> GetSalesTodayAsync()
        {
            var sales = await _machineService.GetSalesByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = new SalesDto();
            foreach (var sale in sales)
            {
                result.Balance += sale.TotalPrice;
                result.ProfitToday += (sale.Timestamp.Date == DateTime.UtcNow.Date) ? sale.TotalPrice / 2 : 0;
                result.ProfitYestarday += (sale.Timestamp.Date == DateTime.UtcNow.AddDays(-1).Date) ? sale.TotalPrice / 2 : 0;
                result.CollectedToday += (sale.Timestamp.Date == DateTime.UtcNow.Date) ? result.ProfitToday / 2 : 0;
                result.CollectedYestarday += (sale.Timestamp.Date == DateTime.UtcNow.AddDays(-1).Date) ? result.ProfitYestarday / 2 : 0;
                result.IsServed = (sale.Timestamp.Date == DateTime.UtcNow.Date) ? Convert.ToBoolean(Random.Shared.Next(0, 2)) : false;
            }

            return result;
        }

        [HttpGet("Sales")]
        [Authorize]
        public async Task<ActionResult<List<Sale>>> GetSalesAsync()
        {
            return await _machineService.GetSalesByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
