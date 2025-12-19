using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Restraunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Сервис для получения статистики и аналитики
    /// </summary>
    public class StatisticsService
    {
        /// <summary>
        /// Получить статистику продаж блюд за период
        /// </summary>
        /// <param name="dateFrom">Начало периода (null = без ограничения)</param>
        /// <param name="dateTo">Конец периода (null = без ограничения)</param>
        /// <returns>Список статистики по каждому блюду</returns>
        public List<DishStatisticsModel> GetDishStatistics(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using var context = new RestrauntContext();

            var query = context.OrderItems
                .Include(oi => oi.Dish)
                .ThenInclude(d => d.Category)
                .Include(oi => oi.Order)
                .AsQueryable();

            // Фильтр по дате заказа
            if (dateFrom.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                query = query.Where(oi => oi.Order.OrderDate >= fromUtc);
            }

            if (dateTo.HasValue)
            {
                var toUtc = DateTime.SpecifyKind(dateTo.Value.AddDays(1).AddSeconds(-1), DateTimeKind.Utc);
                query = query.Where(oi => oi.Order.OrderDate <= toUtc);
            }

            // Исключаем отмененные заказы
            query = query.Where(oi => oi.Order.Status != "отменен");

            // Группируем по блюду и считаем статистику
            var statistics = query
                .GroupBy(oi => new
                {
                    oi.DishId,
                    DishName = oi.Dish.Name,
                    CategoryName = oi.Dish.Category != null ? oi.Dish.Category.Name : null,
                    CurrentPrice = oi.Dish.Price
                })
                .Select(g => new DishStatisticsModel
                {
                    DishId = g.Key.DishId,
                    DishName = g.Key.DishName,
                    CategoryName = g.Key.CategoryName,
                    CurrentPrice = g.Key.CurrentPrice,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.PriceAtOrder * oi.Quantity),
                    OrderCount = g.Select(oi => oi.OrderId).Distinct().Count()
                })
                .OrderByDescending(s => s.TotalRevenue)
                .ToList();

            return statistics;
        }

        /// <summary>
        /// Получить топ N самых популярных блюд по количеству заказов
        /// </summary>
        public List<DishStatisticsModel> GetTopDishesByOrders(int topCount = 10, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            return GetDishStatistics(dateFrom, dateTo)
                .OrderByDescending(s => s.OrderCount)
                .ThenByDescending(s => s.TotalQuantity)
                .Take(topCount)
                .ToList();
        }

        /// <summary>
        /// Получить топ N самых прибыльных блюд по выручке
        /// </summary>
        public List<DishStatisticsModel> GetTopDishesByRevenue(int topCount = 10, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            return GetDishStatistics(dateFrom, dateTo)
                .OrderByDescending(s => s.TotalRevenue)
                .Take(topCount)
                .ToList();
        }

        /// <summary>
        /// Получить статистику продаж по категориям блюд
        /// </summary>
        public List<CategoryStatisticsModel> GetCategoryStatistics(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using var context = new RestrauntContext();

            var query = context.OrderItems
                .Include(oi => oi.Dish)
                .ThenInclude(d => d.Category)
                .Include(oi => oi.Order)
                .AsQueryable();

            if (dateFrom.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                query = query.Where(oi => oi.Order.OrderDate >= fromUtc);
            }

            if (dateTo.HasValue)
            {
                var toUtc = DateTime.SpecifyKind(dateTo.Value.AddDays(1).AddSeconds(-1), DateTimeKind.Utc);
                query = query.Where(oi => oi.Order.OrderDate <= toUtc);
            }

            query = query.Where(oi => oi.Order.Status != "отменен");

            var statistics = query
                .Where(oi => oi.Dish.Category != null)
                .GroupBy(oi => new
                {
                    CategoryId = oi.Dish.Category!.Id,
                    CategoryName = oi.Dish.Category.Name
                })
                .Select(g => new CategoryStatisticsModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.PriceAtOrder * oi.Quantity),
                    DishCount = g.Select(oi => oi.DishId).Distinct().Count()
                })
                .OrderByDescending(s => s.TotalRevenue)
                .ToList();

            return statistics;
        }
    }

    /// <summary>
    /// Модель статистики по категории
    /// </summary>
    public class CategoryStatisticsModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DishCount { get; set; }
    }
}
