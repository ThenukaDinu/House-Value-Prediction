using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Models;

namespace Micro_House_Manage_API.Repository
{
    public class ListingRepository : BaseRepository<Listing>, IListingRepository
    {
        private DataContext _context;
        public ListingRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
