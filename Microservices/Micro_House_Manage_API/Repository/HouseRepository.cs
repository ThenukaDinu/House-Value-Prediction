using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Micro_House_Manage_API.Repository
{
    public class HouseRepository : BaseRepository<House>, IHouseRepository
    {
        private DataContext _context;
        public HouseRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async override Task<ICollection<House>> GetAllAsync()
        {
            return await _context.Houses.Include(h => h.HousePhotos).ToListAsync();
        }

        public async override Task<House> GetByIdAsync(int id)
        {
            return await _context.Houses.Include(h => h.HousePhotos).FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
