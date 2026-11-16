using Microsoft.EntityFrameworkCore;

namespace LapDistance.Data
{
    /// <summary>
    /// Контекст базы данных Entity Framework Core.
    /// База данных – SQLite, файл workouts.db.
    /// </summary>
    public class WorkoutContext : DbContext
    {
        /// <summary>Таблица тренировок.</summary>
        public DbSet<Workout> Workouts { get; set; }

        /// <summary>Таблица кругов.</summary>
        public DbSet<Lap> Laps { get; set; }

        /// <summary>Конструктор, используемый Dependency Injection (параметры задаются в Program.cs).</summary>
        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options)
        {
        }

        /// <summary>Конструктор без параметров (например, для инструментов EF).</summary>
        public WorkoutContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Если контекст создан без параметров – используем базу рядом с программой
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=workouts.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь один-ко-многим: тренировка -> круги, при удалении тренировки круги удаляются
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Laps)
                .WithOne(l => l.Workout)
                .HasForeignKey(l => l.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasIndex(w => w.StartTime);

            modelBuilder.Entity<Lap>()
                .HasIndex(l => new { l.WorkoutId, l.Number });
        }
    }
}
