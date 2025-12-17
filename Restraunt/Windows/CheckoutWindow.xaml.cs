using Restraunt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restraunt.Windows
{
    /// <summary>
    /// Логика взаимодействия для CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        public CheckoutWindow(decimal totalAmount)
        {
            InitializeComponent();

            var vm = new CheckoutViewModel(totalAmount);
            vm.OrderCreated += () => this.Close();

            DataContext = vm;
        }
    }
}
