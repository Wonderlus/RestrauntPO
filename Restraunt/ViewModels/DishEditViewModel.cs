using BLL;
using CommunityToolkit.Mvvm.Input;
using Models;
using Restraunt.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Restraunt.ViewModels
{
    public class DishEditViewModel : ViewModelBase
    {
        private readonly DishService _dishService = new();
        private readonly DishCategoryService _categoryService = new();
        private readonly bool _isEditMode;

        public DishModel Dish { get; }
        public List<DishCategoryModel> Categories { get; }

        public string WindowTitle => _isEditMode ? "Edit dish" : "Add dish";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Action callback for closing the window with result
        public Action<bool>? RequestClose { get; set; }

        // Constructor for CREATE
        public DishEditViewModel() : this(null) { }

        // Constructor for EDIT
        public DishEditViewModel(DishModel? dish)
        {
            _isEditMode = dish != null;
            Dish = dish ?? new DishModel();
            Categories = _categoryService.GetAll();

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Dish.Name))
            {
                MessageBox.Show("Dish name is required");
                return;
            }

            if (Dish.CategoryId == null)
            {
                MessageBox.Show("Select category");
                return;
            }

            if (_isEditMode)
                _dishService.Update(Dish);
            else
                _dishService.Add(Dish);

            RequestClose?.Invoke(true);
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(false);
        }
    }
}
