using Microsoft.EntityFrameworkCore;
using VendingApi.Contexts;
using VendingApi.Dtos;
using VendingApi.Models;
using WEMMApi.Dtos;

namespace WEMMApi.Services
{
    public class MachineService(AppDbContext context)
    {
        AppDbContext _context = context;

        public IQueryable<MachineDto> GetMachineDtosQueryByUserId(string id)
            => _context.VendingMachines.Include(m => m.Model)
                .Include(m => m.Company)
                .Include(m => m.Place)
                .Select(m => new MachineDto()
                {
                    MachineId = m.MachineId,
                    SerialNumber = m.SerialNumber,
                    Name = m.Name,
                    Location = m.Location,
                    UserId = m.UserId,
                    ModelName = m.Model.Name,
                    CompanyName = m.Company.Name,
                    PlaceName = m.Place.Name,
                    InstallDate = m.InstallDate
                }).Where(m => m.UserId == id);

        public IQueryable<MachineDto> ApplyPagination(IQueryable<MachineDto> query,
                                                                   int currentPage,
                                                                   int pageSize)
            => query.Skip((currentPage - 1) * pageSize)
            .Take(pageSize);

        public IQueryable<MachineDto> ApplyFilter(IQueryable<MachineDto> query, string filter)
            => query.Where(m => m.Name.ToLower().Contains(filter.ToLower().Trim()));

        public async Task<EfficiencyResponceDto> GetMashinesEfficiencyAsync(string id)
        {
            var mashines = await _context.VendingMachines.Include(m => m.StatusNavigation).Select(m => new MashinesEfficiencyDto()
            {
                MachineId = m.MachineId,
                UserId = m.UserId,
                StatusName = m.StatusNavigation.Name
            }).Where(m => m.UserId == id).ToListAsync();

            var allMashines = mashines.Count();

            var workingMashines = mashines.Where(m => m.StatusName == "Работает").Count();
            var brokenMashines = mashines.Where(m => m.StatusName == "Сломан").Count();
            var servedMashines = mashines.Where(m => m.StatusName == "Обслуживается").Count();

            return new EfficiencyResponceDto()
            {
                AllMashines = allMashines,
                WorkingMashines = workingMashines,
                BrokenMashines = brokenMashines,
                ServedMashines = servedMashines
            };
        }

        public async Task<List<News>> GetNewsByIdAsync(string id) 
            => await _context.News.Where(n => n.UserId == id).OrderBy(n => n.EventDate).ToListAsync();

        public async Task<List<Sale>> GetSalesByUserId(string userId)
        {
            var machineIds = await _context.VendingMachines
                .Where(m => m.UserId == userId)
                .Select(m => m.MachineId)
                .ToListAsync();
            var result = new List<Sale>();

            foreach (var machineId in machineIds)
            {
                var subResult = await _context.Sales.Where(s => s.MachineId == machineId).ToListAsync();

                result.AddRange(subResult);
            }

            return result;
        }

        public async Task<bool> CreateMachine(VendingMachine machine, string userId)
        {
            try
            {
                await _context.VendingMachines.AddAsync(machine);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
