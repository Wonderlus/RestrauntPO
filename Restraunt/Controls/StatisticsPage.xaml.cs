using Restraunt.ViewModels;
using System.Windows.Controls;

namespace Restraunt.Controls
{
    public partial class StatisticsPage : UserControl
    {
        public StatisticsPage()
        {
            InitializeComponent();
            DataContext = new DishStatisticsViewModel();
        }
    }
}
