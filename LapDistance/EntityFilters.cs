using LapDistance.Data;

namespace LapDistance.Services
{
    /// <summary>
    /// Готовое условие фильтрации: название для интерфейса, текст лямбда-выражения
    /// (для показа пользователю) и сам делегат Predicate&lt;T&gt;.
    /// </summary>
    public sealed class FilterOption<T>
    {
        public string Title { get; }
        public string LambdaText { get; }
        public Predicate<T> Predicate { get; }

        public FilterOption(string title, string lambdaText, Predicate<T> predicate)
        {
            Title = title;
            LambdaText = lambdaText;
            Predicate = predicate;
        }

        public override string ToString() => Title;
    }

    /// <summary>
    /// Наборы условий фильтрации для обеих сущностей приложения.
    /// Каждое условие задано лямбда-выражением и передаётся в Service&lt;T&gt;.Get(Predicate&lt;T&gt;).
    /// </summary>
    public static class EntityFilters
    {
        /// <summary>Условия для тренировок (Service&lt;Workout&gt;).</summary>
        public static readonly IReadOnlyList<FilterOption<Workout>> Workouts = new List<FilterOption<Workout>>
        {
            new("Все тренировки",                       "w => true",                              w => true),
            new("Дистанция больше 3 км",                "w => w.Distance > 3000",                 w => w.Distance > 3000),
            new("Продолжительность больше 30 мин",      "w => w.Duration > 1800",                 w => w.Duration > 1800),
            new("Бег (ActivityType == 3)",              "w => w.ActivityType == 3",               w => w.ActivityType == 3),
            new("Ходьба (ActivityType == 12)",          "w => w.ActivityType == 12",              w => w.ActivityType == 12),
            new("Есть круги",                           "w => w.Laps.Count > 0",                  w => w.Laps.Count > 0),
            new("Без кругов (нет данных варианта 9)",   "w => w.Laps.Count == 0",                 w => w.Laps.Count == 0),
            new("Средний пульс выше 120 уд/мин",        "w => w.AvgHeartRate > 120",              w => w.AvgHeartRate > 120),
            new("Больше 3000 шагов",                    "w => w.StepCount > 3000",                w => w.StepCount > 3000),
            new("Тренировки 2026 года",                 "w => w.StartTime.Year == 2026",          w => w.StartTime.Year == 2026),
            new("Вечерние (начало после 19:00)",        "w => w.StartTime.Hour >= 19",            w => w.StartTime.Hour >= 19),
        };

        /// <summary>Условия для кругов (Service&lt;Lap&gt;).</summary>
        public static readonly IReadOnlyList<FilterOption<Lap>> Laps = new List<FilterOption<Lap>>
        {
            new("Все круги",                            "l => true",                              l => true),
            new("Автокруги",                            "l => l.LapType == \"Autolap\"",          l => l.LapType == Lap.TypeAuto),
            new("Ручные круги",                         "l => l.LapType == \"Lap\"",              l => l.LapType == Lap.TypeManual),
            new("Полные автокруги (ровно 1000 м)",      "l => l.IsAutoLap && l.Distance == 1000", l => l.IsAutoLap && l.Distance == 1000),
            new("Неполные автокруги (< 1000 м)",        "l => l.IsAutoLap && l.Distance < 1000",  l => l.IsAutoLap && l.Distance < 1000),
            new("Короткие круги (< 500 м)",             "l => l.Distance < 500",                  l => l.Distance < 500),
            new("Длинные круги (> 1200 м)",             "l => l.Distance > 1200",                 l => l.Distance > 1200),
            new("Быстрые круги (> 10 км/ч)",            "l => l.AvgSpeed * 3.6 > 10",             l => l.AvgSpeed * 3.6 > 10),
            new("Долгие круги (> 6 мин)",               "l => l.Duration > 360",                  l => l.Duration > 360),
            new("Круги с пульсом выше 140",             "l => l.AvgHeartRate > 140",              l => l.AvgHeartRate > 140),
        };
    }
}
