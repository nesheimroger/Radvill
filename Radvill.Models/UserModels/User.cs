using System;

namespace Radvill.Models.UserModels
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }

        public virtual AdvisorProfile AdvisorProfile { get; set; }
    }
}