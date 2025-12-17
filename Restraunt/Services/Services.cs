using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restraunt.Services
{
    public static class Session
    {
        public static CustomerEntity? CurrentUser { get; set; }

        public static bool IsAdmin =>
            CurrentUser != null && CurrentUser.IsAdmin;
    }
}
