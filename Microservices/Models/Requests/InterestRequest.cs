using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.PublicEnum;

namespace Models.Requests
{
    public class InterestRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public InterestType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
