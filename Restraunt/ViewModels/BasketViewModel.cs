using BLL;
using CommunityToolkit.Mvvm.Input;
using Models;
using Restraunt.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Restraunt.ViewModels
{
    public class BasketViewModel : INotifyPropertyChanged
    {
        private readonly BasketService _basketService = new();

        public BasketViewModel()
        {
            IncreaseCommand = new RelayCommand<BasketItemModel>(Increase);
            DecreaseCommand = new RelayCommand<BasketItemModel>(Decrease);
            RemoveCommand = new RelayCommand<BasketItemModel>(Remove);
        }

        private List<BasketItemModel> _items = new();
        public List<BasketItemModel> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public decimal TotalPrice => Items.Sum(i => i.Total);

        public IRelayCommand<BasketItemModel> IncreaseCommand { get; }
        public IRelayCommand<BasketItemModel> DecreaseCommand { get; }
        public IRelayCommand<BasketItemModel> RemoveCommand { get; }

        public void LoadBasket()
        {
            if (Session.CurrentUser == null) return;
            Items = _basketService.GetBasket(Session.CurrentUser.Id);
        }

        private void Increase(BasketItemModel? item)
        {
            if (item == null || Session.CurrentUser == null) return;
            _basketService.Increase(Session.CurrentUser.Id, item.DishId);
            LoadBasket();
        }

        private void Decrease(BasketItemModel? item)
        {
            if (item == null || Session.CurrentUser == null) return;
            _basketService.Decrease(Session.CurrentUser.Id, item.DishId);
            LoadBasket();
        }

        private void Remove(BasketItemModel? item)
        {
            if (item == null || Session.CurrentUser == null) return;
            _basketService.Remove(Session.CurrentUser.Id, item.DishId);
            LoadBasket();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
