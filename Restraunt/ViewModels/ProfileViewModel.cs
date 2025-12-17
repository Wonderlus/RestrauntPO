using BLL;
using Restraunt.Services;
using System.Windows;

namespace Restraunt.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly UserService _userService = new();

        private string _fullName = "";
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _phone = "";
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public ProfileViewModel()
        {
            if (Session.CurrentUser == null) return;

            FullName = Session.CurrentUser.FullName;
            Phone = Session.CurrentUser.Phone;
            Email = Session.CurrentUser.Email;
        }

        public void Save()
        {
            if (Session.CurrentUser == null)
                return;

            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            try
            {
                _userService.UpdateUser(
                    Session.CurrentUser.Id,
                    FullName,
                    Phone,
                    Email
                );

                // обновляем текущую сессию
                Session.CurrentUser.FullName = FullName;
                Session.CurrentUser.Phone = Phone;
                Session.CurrentUser.Email = Email;
                MessageBox.Show(FullName);
                // 🔥 ВАЖНО: уведомляем AppViewModel
                Session.NotifyUserUpdated();



            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
