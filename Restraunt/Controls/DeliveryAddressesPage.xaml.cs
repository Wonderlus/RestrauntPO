using Restraunt.ViewModels;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class DeliveryAddressesPage : UserControl
    {
        public DeliveryAddressesPage()
        {
            InitializeComponent();
            DataContext = new DeliveryAddressesViewModel();
        }
    }
}
