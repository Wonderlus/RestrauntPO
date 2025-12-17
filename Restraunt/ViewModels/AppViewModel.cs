using Restraunt.Services;
using Restraunt.ViewModels;

public class AppViewModel : ViewModelBase
{
    public AppViewModel()
    {
        Session.CurrentUserChanged += RefreshUser;
    }

    public string UserName => Session.CurrentUser?.FullName ?? "";
    public string UserPhone => Session.CurrentUser?.Phone ?? "";
    public string UserEmail => Session.CurrentUser?.Email ?? "";

    public bool IsAuthenticated => Session.CurrentUser != null;
    public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;

    public void RefreshUser()
    {
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(UserPhone));
        OnPropertyChanged(nameof(UserEmail));
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsAdmin));
    }
}

