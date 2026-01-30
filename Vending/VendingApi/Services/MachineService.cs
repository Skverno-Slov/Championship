using Microsoft.EntityFrameworkCore;
using VendingApi.Contexts;
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

        public async Task<bool> CreateMachine(VendingMachine machine, string userId)
        {
            try
            {
                var guid = Guid.NewGuid();

                machine.MachineId = guid.ToString();
                await _context.VendingMachines.AddAsync(machine);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
