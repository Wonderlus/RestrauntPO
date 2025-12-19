using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Restraunt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем и настраиваем базу данных при запуске приложения
            InitializeDatabase();
        }

        /// <summary>
        /// Инициализирует базу данных: создает БД, если её нет, и применяет миграции
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                using var context = new RestrauntContext();
                
                // Проверяем, существует ли база данных
                if (!context.Database.CanConnect())
                {
                    // Создаем базу данных и применяем все миграции
                    context.Database.Migrate();
                }
                else
                {
                    // Если БД существует, применяем ожидающие миграции
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение, но не блокируем запуск приложения
                MessageBox.Show(
                    $"Ошибка при инициализации базы данных:\n{ex.Message}\n\n" +
                    "Убедитесь, что PostgreSQL запущен и настройки подключения корректны.",
                    "Ошибка базы данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }

}
