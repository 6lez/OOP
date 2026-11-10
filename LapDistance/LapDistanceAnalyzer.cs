using SuuntoLapDistance.Models;

namespace SuuntoLapDistance.Services
{
    /// <summary>Какие круги включать в анализ.</summary>
    public enum LapFilter
    {
        /// <summary>Ручные круги (Type = "Lap").</summary>
        Manual,
        /// <summary>Автокруги (Type = "Autolap").</summary>
        Auto,
        /// <summary>Все круги (Lap + Autolap).</summary>
        All
    }

    /// <summary>
    /// Строка таблицы анализа дистанции по кругам (вариант 9).
    /// Объекты этого класса отображаются в DataGridView.
    /// </summary>
    public class LapDistanceRow
    {
        /// <summary>Порядковый номер круга.</summary>
        public int Number { get; set; }

        /// <summary>Тип круга (ручной / автокруг).</summary>
        public string LapType { get; set; } = "";

        /// <summary>Время окончания круга.</summary>
        public string EndTime { get; set; } = "";

        /// <summary>Продолжительность круга (мм:сс).</summary>
        public string DurationText { get; set; } = "";

        /// <summary>Дистанция круга, м.</summary>
        public double Distance { get; set; }

        /// <summary>Отклонение дистанции круга от средней дистанции, м.</summary>
        public double DeviationFromAverage { get; set; }

        /// <summary>Отклонение от средней дистанции, %.</summary>
        public double DeviationFromAveragePercent { get; set; }

        /// <summary>Разница с предыдущим кругом, м (null для первого круга).</summary>
        public double? DifferenceFromPrevious { get; set; }

        /// <summary>Доля круга в суммарной дистанции кругов, %.</summary>
        public double ShareOfTotal { get; set; }

        /// <summary>Дистанция нарастающим итогом, м.</summary>
        public double CumulativeDistance { get; set; }

        /// <summary>Средняя скорость на круге, км/ч (если есть).</summary>
        public double? AverageSpeedKmh { get; set; }

        /// <summary>Примечание (самый длинный / короткий / неполный автокруг).</summary>
        public string Note { get; set; } = "";

        public bool IsLongest { get; set; }
        public bool IsShortest { get; set; }
    }

    /// <summary>Сводные показатели по кругам.</summary>
    public class LapDistanceSummary
    {
        public int Count { get; set; }
        public double TotalDistance { get; set; }
        public double AverageDistance { get; set; }
        public double MinDistance { get; set; }
        public int MinLapNumber { get; set; }
        public double MaxDistance { get; set; }
        public int MaxLapNumber { get; set; }
        /// <summary>Размах: разница между самым длинным и самым коротким кругом, м.</summary>
        public double Spread => MaxDistance - MinDistance;
        /// <summary>Размах в процентах от средней дистанции.</summary>
        public double SpreadPercent => AverageDistance > 0 ? Spread / AverageDistance * 100 : 0;
        /// <summary>Среднеквадратичное отклонение дистанции кругов, м.</summary>
        public double StdDeviation { get; set; }
    }

    /// <summary>Результат анализа.</summary>
    public class LapDistanceResult
    {
        public List<LapDistanceRow> Rows { get; } = new();
        public LapDistanceSummary? Summary { get; set; }
        public bool HasData => Rows.Count > 0;
    }

    /// <summary>
    /// Вариант 9. Анализ дистанции по кругам:
    /// сравнение пройденной дистанции по кругам, выявление различий между интервалами.
    /// Источник данных – массив Windows, параметр – Distance.
    /// </summary>
    public static class LapDistanceAnalyzer
    {
        /// <summary>Возвращает все окна-круги тренировки (Lap и Autolap) в порядке их окончания.</summary>
        public static List<WindowEntry> GetLaps(DeviceLog log, LapFilter filter)
        {
            if (log.Windows == null) return new List<WindowEntry>();

            IEnumerable<WindowEntry> laps = log.Windows.Where(w => w.Window != null && w.Window.IsLap);

            laps = filter switch
            {
                LapFilter.Manual => laps.Where(w => w.Window!.IsManualLap),
                LapFilter.Auto => laps.Where(w => w.Window!.IsAutoLap),
                _ => laps
            };

            return laps.OrderBy(w => w.Time ?? DateTimeOffset.MaxValue).ToList();
        }

        public static bool HasManualLaps(DeviceLog log) =>
            log.Windows?.Any(w => w.Window?.IsManualLap == true) == true;

        public static bool HasAutoLaps(DeviceLog log) =>
            log.Windows?.Any(w => w.Window?.IsAutoLap == true) == true;

        /// <summary>Выполняет анализ дистанции по кругам.</summary>
        public static LapDistanceResult Analyze(DeviceLog log, LapFilter filter)
        {
            var result = new LapDistanceResult();

            // Берём только круги, для которых известна дистанция
            var laps = GetLaps(log, filter)
                .Where(w => w.Window!.Distance.HasValue)
                .ToList();

            if (laps.Count == 0)
                return result;

            double? autoLapDistance = log.Header?.Settings?.AutoLap?.Distance;

            double total = laps.Sum(w => w.Window!.Distance!.Value);
            double average = total / laps.Count;
            double min = laps.Min(w => w.Window!.Distance!.Value);
            double max = laps.Max(w => w.Window!.Distance!.Value);
            double variance = laps.Average(w => Math.Pow(w.Window!.Distance!.Value - average, 2));

            double cumulative = 0;
            double? previous = null;
            int minLap = 0, maxLap = 0;

            for (int i = 0; i < laps.Count; i++)
            {
                WorkoutWindow w = laps[i].Window!;
                double distance = w.Distance!.Value;
                cumulative += distance;

                var row = new LapDistanceRow
                {
                    Number = i + 1,
                    LapType = w.IsAutoLap ? "Автокруг" : "Ручной",
                    EndTime = laps[i].Time?.ToString("HH:mm:ss") ?? "—",
                    DurationText = FormatDuration(w.Duration),
                    Distance = distance,
                    DeviationFromAverage = distance - average,
                    DeviationFromAveragePercent = average > 0 ? (distance - average) / average * 100 : 0,
                    DifferenceFromPrevious = previous.HasValue ? distance - previous.Value : null,
                    ShareOfTotal = total > 0 ? distance / total * 100 : 0,
                    CumulativeDistance = cumulative,
                    AverageSpeedKmh = w.Speed?.FirstOrDefault()?.Avg is double v ? v * 3.6 : null
                };

                var notes = new List<string>();
                // Отмечаем только первый круг с максимальной/минимальной дистанцией
                if (distance == max && maxLap == 0 && laps.Count > 1)
                {
                    row.IsLongest = true;
                    maxLap = row.Number;
                    notes.Add("самый длинный");
                }
                if (distance == min && minLap == 0 && laps.Count > 1)
                {
                    row.IsShortest = true;
                    minLap = row.Number;
                    notes.Add("самый короткий");
                }
                // Последний автокруг обычно короче заданной дистанции автокруга
                if (w.IsAutoLap && autoLapDistance is > 0 && distance < autoLapDistance.Value * 0.98)
                    notes.Add($"неполный автокруг (из {autoLapDistance.Value:N0} м)");

                row.Note = string.Join(", ", notes);
                result.Rows.Add(row);
                previous = distance;
            }

            result.Summary = new LapDistanceSummary
            {
                Count = laps.Count,
                TotalDistance = total,
                AverageDistance = average,
                MinDistance = min,
                MinLapNumber = minLap == 0 ? 1 : minLap,
                MaxDistance = max,
                MaxLapNumber = maxLap == 0 ? 1 : maxLap,
                StdDeviation = Math.Sqrt(variance)
            };

            return result;
        }

        /// <summary>Форматирует длительность в секундах как ч:мм:сс или мм:сс.</summary>
        public static string FormatDuration(double? seconds)
        {
            if (seconds == null) return "—";
            var ts = TimeSpan.FromSeconds(seconds.Value);
            return ts.TotalHours >= 1
                ? $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}
