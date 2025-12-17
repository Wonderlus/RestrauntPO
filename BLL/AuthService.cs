using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL
{
    public class AuthService
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public CustomerEntity? Login(string phone, string password)
        {
            using var context = new RestrauntContext();

            return context.Customers
                .FirstOrDefault(c =>
                    c.Phone == phone &&
                    c.Password == password
                );
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        public bool Register(string fullName, string phone, string email, string password)
        {
            using var context = new RestrauntContext();

            if (context.Customers.Any(c => c.Phone == phone))
                return false;

            var customer = new CustomerEntity
            {
                FullName = fullName,
                Phone = phone,
                Email = email,
                Password = password, // ❗ храним как есть
                IsAdmin = false,
                RegistrationDate = DateTime.UtcNow

            };

            context.Customers.Add(customer);
            context.SaveChanges();

            return true;
        }
    }
}
