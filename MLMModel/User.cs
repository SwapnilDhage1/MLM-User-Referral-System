using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLMModel
{

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string MobileNumber { get; set; }
        public int? SponsorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
