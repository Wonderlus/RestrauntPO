using System;

namespace Restraunt.Models
{
    /// <summary>
    /// Модель статистики продаж блюда
    /// </summary>
    public class DishStatisticsModel
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string? CategoryName { get; set; }
        public decimal CurrentPrice { get; set; }
        
        /// <summary>
        /// Общее количество заказанных порций
        /// </summary>
        public int TotalQuantity { get; set; }
        
        /// <summary>
        /// Общая выручка от продажи этого блюда
        /// </summary>
        public decimal TotalRevenue { get; set; }
        
        /// <summary>
        /// Количество уникальных заказов, в которых было это блюдо
        /// </summary>
        public int OrderCount { get; set; }
        
        /// <summary>
        /// Средняя цена продажи (с учетом цен на момент заказа)
        /// </summary>
        public decimal AveragePrice => TotalQuantity > 0 ? TotalRevenue / TotalQuantity : 0;
        
        /// <summary>
        /// Среднее количество порций в одном заказе
        /// </summary>
        public decimal AverageQuantityPerOrder => OrderCount > 0 ? (decimal)TotalQuantity / OrderCount : 0;
    }
}
