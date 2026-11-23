using LapDistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LapDistance.Services
{
    /// <summary>
    /// Обобщённый сервис работы с данными (практическая работа № 3).
    ///
    /// Один и тот же класс используется для разных сущностей базы данных:
    /// Service&lt;Workout&gt; – тренировки, Service&lt;Lap&gt; – круги.
    /// Доступ к таблице нужного типа выполняется через DbContext.Set&lt;T&gt;().
    ///
    /// Ограничение where T : class требуется методом Set&lt;T&gt;() (сущности EF Core –
    /// ссылочные типы) и не позволяет подставить вместо T, например, int.
    /// </summary>
    /// <typeparam name="T">Тип сущности (класс из папки Data).</typeparam>
    public class Service<T> where T : class
    {
        private readonly WorkoutContext _context;

        /// <summary>Контекст передаётся через Dependency Injection (см. Program.cs).</summary>
        public Service(WorkoutContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------------
        //  Основные операции
        // ------------------------------------------------------------------

        /// <summary>Все объекты типа T.</summary>
        public List<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// Все объекты типа T вместе со связанными данными.
        /// Пример: workoutService.GetAll(w => w.Laps) – тренировки вместе с кругами.
        /// </summary>
        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return Query(includes).ToList();
        }

        /// <summary>Объект по идентификатору (первичному ключу) или null.</summary>
        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Объект по идентификатору вместе со связанными данными.
        /// Пример: workoutService.GetById(5, w => w.Laps).
        /// Предполагается, что первичный ключ сущности называется "Id".
        /// </summary>
        public T? GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            return Query(includes).FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        /// <summary>Добавляет объект (вместе со связанными объектами) и сохраняет изменения.</summary>
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        /// <summary>Количество объектов типа T в базе.</summary>
        public int Count()
        {
            return _context.Set<T>().Count();
        }

        // ------------------------------------------------------------------
        //  Фильтрация с помощью делегата Predicate<T>
        // ------------------------------------------------------------------

        /// <summary>
        /// Объекты, удовлетворяющие условию. Условие передаётся лямбда-выражением:
        /// workoutService.Get(w => w.Distance > 3000).
        /// Данные читаются из базы, затем фильтруются в памяти методом List&lt;T&gt;.FindAll,
        /// который принимает Predicate&lt;T&gt;.
        /// </summary>
        public List<T> Get(Predicate<T> predicate)
        {
            return _context.Set<T>().AsNoTracking().ToList().FindAll(predicate);
        }

        /// <summary>То же, но со связанными данными: lapService.Get(l => l.Distance &lt; 1000, l => l.Workout!).</summary>
        public List<T> Get(Predicate<T> predicate, params Expression<Func<T, object>>[] includes)
        {
            return Query(includes).ToList().FindAll(predicate);
        }

        /// <summary>Есть ли хотя бы один объект, удовлетворяющий условию.</summary>
        public bool Any(Predicate<T> predicate)
        {
            return Get(predicate).Count > 0;
        }

        // ------------------------------------------------------------------
        //  Служебное
        // ------------------------------------------------------------------

        /// <summary>Полный путь к файлу базы данных (для вывода пользователю).</summary>
        public string? GetDatabasePath()
        {
            string? dataSource = _context.Database.GetDbConnection().DataSource;
            return dataSource == null ? null : Path.GetFullPath(dataSource);
        }

        /// <summary>Запрос к таблице T без отслеживания изменений, с подключением связанных данных.</summary>
        private IQueryable<T> Query(Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);
            return query;
        }
    }
}
