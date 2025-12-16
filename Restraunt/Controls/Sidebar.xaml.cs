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
        private string _active = "Dishes";

        public Sidebar()
        {
            InitializeComponent();

            MenuList.ItemsSource = new List<string>
            {
                "Dishes",
                "Customers",
                "Orders"
            };

            //UpdateActiveStyles();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Tag is string)
            {
                _active = (string)btn.Tag;
                UpdateActiveStyles();
            }
        }

        private void UpdateActiveStyles()
        {
            foreach (var item in MenuList.Items)
            {
                var container = MenuList.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container == null) continue;

                var btn = FindVisualChild<Button>(container);
                if (btn == null) continue;

                if ((string)item == _active)
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xED, 0xD7));
                    btn.FontWeight = FontWeights.SemiBold;
                }
                else
                {
                    btn.Background = Brushes.Transparent;
                    btn.FontWeight = FontWeights.Normal;
                }
            }
        }

        // ★ Вариант без nullable и с обычным return null
        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj == null)
                return null;

            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                T childAsT = child as T;
                if (childAsT != null)
                    return childAsT;

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }
    }
}
