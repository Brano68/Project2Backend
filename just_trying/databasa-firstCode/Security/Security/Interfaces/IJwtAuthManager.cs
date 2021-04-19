using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Interfaces
{
    public interface IJwtAuthManager
    {
        public string Authenticate(string login, string password);
    }
}
