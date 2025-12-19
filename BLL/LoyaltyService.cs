using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Сервис для работы с программой лояльности и баллами
    /// </summary>
    public class LoyaltyService
    {
        // Настройки программы лояльности
        private const decimal POINTS_EARNING_RATE = 0.01m; // 1% от суммы заказа
        private const decimal POINTS_TO_RUBLES_RATE = 1m; // 1 балл = 1 рубль

        /// <summary>
        /// Получить текущий баланс баллов клиента
        /// </summary>
        public int GetCustomerPoints(int customerId)
        {
            using var context = new RestrauntContext();
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
            return customer?.LoyaltyPoints ?? 0;
        }

        /// <summary>
        /// Рассчитать количество баллов, которое будет начислено за заказ
        /// </summary>
        /// <param name="orderAmount">Сумма заказа после всех скидок</param>
        /// <returns>Количество баллов для начисления</returns>
        public int CalculatePointsToEarn(decimal orderAmount)
        {
            // Начисляем 1% от суммы заказа, округляем вниз
            return (int)Math.Floor(orderAmount * POINTS_EARNING_RATE);
        }

        /// <summary>
        /// Рассчитать максимальную скидку в рублях, которую можно получить за счет баллов
        /// </summary>
        /// <param name="availablePoints">Доступное количество баллов</param>
        /// <param name="orderAmount">Сумма заказа</param>
        /// <returns>Максимальная скидка в рублях</returns>
        public decimal CalculateMaxDiscountFromPoints(int availablePoints, decimal orderAmount)
        {
            // Нельзя использовать больше баллов, чем сумма заказа
            decimal maxDiscountFromPoints = availablePoints * POINTS_TO_RUBLES_RATE;
            return Math.Min(maxDiscountFromPoints, orderAmount);
        }

        /// <summary>
        /// Рассчитать количество баллов, необходимое для получения определенной скидки
        /// </summary>
        /// <param name="discountAmount">Желаемая скидка в рублях</param>
        /// <returns>Необходимое количество баллов</returns>
        public int CalculatePointsNeededForDiscount(decimal discountAmount)
        {
            return (int)Math.Ceiling(discountAmount / POINTS_TO_RUBLES_RATE);
        }

        /// <summary>
        /// Начислить баллы клиенту
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <param name="points">Количество баллов для начисления</param>
        public void AddPoints(int customerId, int points)
        {
            if (points <= 0) return;

            using var context = new RestrauntContext();
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new Exception("Клиент не найден");

            customer.LoyaltyPoints += points;
            context.SaveChanges();
        }

        /// <summary>
        /// Списать баллы у клиента
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <param name="points">Количество баллов для списания</param>
        /// <exception cref="Exception">Если у клиента недостаточно баллов</exception>
        public void DeductPoints(int customerId, int points)
        {
            if (points <= 0) return;

            using var context = new RestrauntContext();
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new Exception("Клиент не найден");

            if (customer.LoyaltyPoints < points)
                throw new Exception("Недостаточно баллов для списания");

            customer.LoyaltyPoints -= points;
            context.SaveChanges();
        }

    }
}
