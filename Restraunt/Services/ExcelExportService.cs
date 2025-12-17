using ClosedXML.Excel;
using DAL.Entities;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restraunt.Services
{
    public static class ExcelExportService
    {
        public static void ExportOrders(List<OrderEntity> orders)
        {
            if (orders.Count == 0)
            {
                System.Windows.MessageBox.Show(
                    "Нет заказов за выбранный период",
                    "Экспорт",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"orders_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Заказы");

            // Заголовки
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Дата";
            ws.Cell(1, 3).Value = "Клиент";
            ws.Cell(1, 4).Value = "Тип";
            ws.Cell(1, 5).Value = "Статус";
            ws.Cell(1, 6).Value = "Сумма";
            ws.Cell(1, 7).Value = "Скидка %";
            ws.Cell(1, 8).Value = "Итого";
            ws.Cell(1, 9).Value = "Адрес";

            int row = 2;

            foreach (var o in orders)
            {
                ws.Cell(row, 1).Value = o.Id;
                ws.Cell(row, 2).Value = o.OrderDate;
                ws.Cell(row, 3).Value = o.Customer.FullName;
                ws.Cell(row, 4).Value = o.OrderType;
                ws.Cell(row, 5).Value = o.Status;
                ws.Cell(row, 6).Value = o.TotalAmount / o.Discount;
                ws.Cell(row, 7).Value = (1 - o.Discount) * 100;
                ws.Cell(row, 8).Value = o.TotalAmount;
                ws.Cell(row, 9).Value = o.DeliveryAddress?.Address;
                row++;
            }

            ws.Columns().AdjustToContents();
            ws.Row(1).Style.Font.Bold = true;

            workbook.SaveAs(dialog.FileName);

            System.Windows.MessageBox.Show(
                "Экспорт успешно выполнен",
                "Excel",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
    }
}
