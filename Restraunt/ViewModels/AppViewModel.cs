using Restraunt.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restraunt.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin)));
        }
    }
}
