using DAL.Entities;
using System;

namespace Restraunt.Services
{
    public static class Session
    {
        private static CustomerEntity? _currentUser;
        public static CustomerEntity? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }

        public static event Action? CurrentUserChanged;

        // если меняем поля у текущего пользователя (FullName/Phone/Email), вызываем это:
        public static void NotifyUserUpdated()
        {
            CurrentUserChanged?.Invoke();
        }
    }
}
