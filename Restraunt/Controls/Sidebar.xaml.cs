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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restraunt.Controls
{
    /// <summary>
    /// Логика взаимодействия для Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
            SetMenuSidebar();
        }

        public void SetMenuSidebar()
        {
            Content = new MenuSidebar();
        }
        public void SetOrdersSidebar()
        {
            Content = new OrdersSidebar();
        }
        //public void SetOrdersSidebar()
        //{
        //    Content = new OrdersSidebar();
        //}

        public void SetEmptySidebar()
        {
            Content = null;
        }
    }
}
