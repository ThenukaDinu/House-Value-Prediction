using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Models;

namespace Micro_House_Manage_API.Repository
{
    public class HousePhotoRepository : BaseRepository<HousePhoto>, IHousePhotoRepository
    {
        private DataContext _context;
        public HousePhotoRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
