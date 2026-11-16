using Microsoft.EntityFrameworkCore;
using LapDistance.Data;

namespace LapDistance.Services
{
    /// <summary>
    /// Сервисный слой для работы с тренировками в базе данных.
    /// Форма обращается только к методам этого класса, а он – к WorkoutContext.
    /// Реализованы только операции, необходимые для сохранения и просмотра.
    /// </summary>
    public class WorkoutService
    {
        private readonly WorkoutContext _context;

        /// <summary>Контекст передаётся через Dependency Injection (см. Program.cs).</summary>
        public WorkoutService(WorkoutContext context)
        {
            _context = context;
        }

        /// <summary>Создаёт базу данных и таблицы, если их ещё нет.</summary>
        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureCreated();
        }

        /// <summary>Полный путь к файлу базы данных (для вывода пользователю).</summary>
        public string? GetDatabasePath()
        {
            string? dataSource = _context.Database.GetDbConnection().DataSource;
            return dataSource == null ? null : Path.GetFullPath(dataSource);
        }

        /// <summary>Список всех сохранённых тренировок (без кругов), новые – первыми.</summary>
        public List<Workout> GetAll()
        {
            return _context.Workouts
                .AsNoTracking()
                .Include(w => w.Laps)          // нужны для подсчёта кругов в подписи
                .OrderByDescending(w => w.StartTime)
                .ToList();
        }

        /// <summary>Тренировка с кругами по идентификатору.</summary>
        public Workout? GetById(int id)
        {
            Workout? workout = _context.Workouts
                .AsNoTracking()
                .Include(w => w.Laps)
                .FirstOrDefault(w => w.Id == id);

            // Круги – в порядке их окончания
            workout?.Laps.Sort((a, b) =>
            {
                int byTime = Nullable.Compare(a.EndTime, b.EndTime);
                return byTime != 0 ? byTime : a.Number.CompareTo(b.Number);
            });

            return workout;
        }

        /// <summary>Проверяет, была ли эта тренировка уже сохранена (по времени начала и виду активности).</summary>
        public bool Exists(Workout workout)
        {
            return _context.Workouts.Any(w =>
                w.StartTime == workout.StartTime && w.ActivityType == workout.ActivityType);
        }

        /// <summary>Сохраняет тренировку вместе с кругами. Возвращает присвоенный Id.</summary>
        public int Add(Workout workout)
        {
            workout.ImportedAt = DateTime.Now;
            _context.Workouts.Add(workout);     // круги добавятся каскадно через навигационное свойство
            _context.SaveChanges();
            return workout.Id;
        }

        /// <summary>Количество сохранённых тренировок.</summary>
        public int Count() => _context.Workouts.Count();
    }
}
