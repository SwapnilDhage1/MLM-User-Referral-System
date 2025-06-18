using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLMModel
{
    public class ReferralDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public List<ReferralDto> Referrals { get; set; } = new List<ReferralDto>();
    }

}
