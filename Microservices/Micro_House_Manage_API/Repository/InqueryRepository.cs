using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Models;

namespace Micro_House_Manage_API.Repository
{
    public class InqueryRepository : BaseRepository<Inquiry>, IInquiryRepository
    {
        private DataContext _context;
        public InqueryRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
