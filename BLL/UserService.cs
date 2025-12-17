using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserService
    {
        public List<CustomerEntity> GetAllUsers()
        {
            using var context = new RestrauntContext();

            return context.Customers
                .OrderBy(c => c.FullName)
                .ToList();
        }

        public void SetAdmin(int userId, bool isAdmin)
        {
            using var context = new RestrauntContext();

            var user = context.Customers.FirstOrDefault(c => c.Id == userId);
            if (user == null) return;

            user.IsAdmin = isAdmin;
            context.SaveChanges();
        }

        public void Delete(int userId)
        {
            using var context = new RestrauntContext();

            var user = context.Customers.FirstOrDefault(c => c.Id == userId);
            if (user == null) return;

            context.Customers.Remove(user);
            context.SaveChanges();
        }
    }
}
