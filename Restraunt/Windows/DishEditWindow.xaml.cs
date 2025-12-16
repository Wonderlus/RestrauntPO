using BLL;
using Models;
using Restraunt.Models;
using System.Collections.Generic;
using System.Windows;

namespace Restraunt.Windows
{
    public partial class DishEditWindow : Window
    {
        private readonly DishService _dishService;
        private readonly DishCategoryService _categoryService;
        private readonly bool _isEditMode;

        public DishModel Dish { get; }
        public List<DishCategoryModel> Categories { get; }

        // CREATE
        public DishEditWindow()
        {
            InitializeComponent();

            _dishService = new DishService();
            _categoryService = new DishCategoryService();

            Dish = new DishModel();
            Categories = _categoryService.GetAll();

            DataContext = this;
            _isEditMode = false;

            Title = "Add dish";
        }

        // EDIT
        public DishEditWindow(DishModel dish)
        {
            InitializeComponent();

            _dishService = new DishService();
            _categoryService = new DishCategoryService();

            Dish = dish;
            Categories = _categoryService.GetAll();

            DataContext = this;
            _isEditMode = true;

            Title = "Edit dish";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
