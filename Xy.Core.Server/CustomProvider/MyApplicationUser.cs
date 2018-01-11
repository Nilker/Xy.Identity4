using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Xy.Core.Server.CustomProvider
{
    public class MyApplicationUser : IIdentity
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual String PasswordHash { get; set; }
        public string NormalizedUserName { get; internal set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
