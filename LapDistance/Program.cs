using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LapDistance.Data;
using LapDistance.Services;

namespace LapDistance
{
    internal static class Program
    {
        /// <summary>Точка входа приложения.</summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Файл базы данных лежит рядом с exe-файлом программы (bin\Debug\net8.0-windows\workouts.db)
            string dbPath = Path.Combine(AppContext.BaseDirectory, "workouts.db");

            // ---------- Регистрация зависимостей (Dependency Injection) ----------
            var services = new ServiceCollection();

            // 1. DbContext (Entity Framework Core + SQLite)
            services.AddDbContext<WorkoutContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // 2. Сервис работы с тренировками (получает WorkoutContext через конструктор)
            services.AddTransient<WorkoutService>();

            // 3. Форма, использующая сервис (получает WorkoutService через конструктор)
            services.AddTransient<MainForm>();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Создаём базу данных и таблицы при первом запуске
            try
            {
                serviceProvider.GetRequiredService<WorkoutService>().EnsureDatabaseCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось создать или открыть базу данных:\n" + ex.Message,
                    "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Экземпляр формы создаёт контейнер DI, подставляя все зависимости
            MainForm form = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(form);
        }
    }
}
