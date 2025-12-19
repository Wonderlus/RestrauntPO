using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Restraunt.Messages;
using Restraunt.Services;
using Restraunt.ViewModels;
using System.Windows.Input;

public class AppViewModel : ViewModelBase
{
    public AppViewModel()
    {
        Session.CurrentUserChanged += RefreshUser;

        // Navigation commands
        NavigateToMenuCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Menu")));
        
        NavigateToBasketCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Basket")));
        
        NavigateToOrdersCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Orders")));
        
        NavigateToProfileCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Profile")));
        
        NavigateToCustomersCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Customers")));
        
        NavigateToAddressesCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new NavigateToPageMessage("Addresses")));
        
        LogoutCommand = new RelayCommand(() => 
            WeakReferenceMessenger.Default.Send(new LogoutMessage()));
    }

    // User properties
    public string UserName => Session.CurrentUser?.FullName ?? "";
    public string UserPhone => Session.CurrentUser?.Phone ?? "";
    public string UserEmail => Session.CurrentUser?.Email ?? "";

    public bool IsAuthenticated => Session.CurrentUser != null;
    public bool IsAdmin => Session.CurrentUser?.IsAdmin == true;

    // Navigation commands
    public ICommand NavigateToMenuCommand { get; }
    public ICommand NavigateToBasketCommand { get; }
    public ICommand NavigateToOrdersCommand { get; }
    public ICommand NavigateToProfileCommand { get; }
    public ICommand NavigateToCustomersCommand { get; }
    public ICommand NavigateToAddressesCommand { get; }
    public ICommand LogoutCommand { get; }

    public void RefreshUser()
    {
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(UserPhone));
        OnPropertyChanged(nameof(UserEmail));
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsAdmin));
    }
}
