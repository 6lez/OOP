using LapDistance.Data;
using LapDistance.Models;

namespace LapDistance.Services
{
    /// <summary>
    /// Преобразует десериализованный JSON-файл Suunto (классы Models)
    /// в сущности базы данных (классы Data): Workout + список Lap.
    /// </summary>
    public static class WorkoutMapper
    {
        public static Workout FromJson(SuuntoWorkoutFile file, string fileName)
        {
            DeviceLog log = file.DeviceLog ?? throw new ArgumentException("Нет раздела DeviceLog", nameof(file));
            WorkoutHeader h = log.Header ?? throw new ArgumentException("Нет раздела Header", nameof(file));

            // Сводное окно "Activity" содержит средний/максимальный пульс и среднюю скорость за всю тренировку
            WorkoutWindow? activity = log.Windows?
                .Select(w => w.Window)
                .FirstOrDefault(w => w != null &&
                    string.Equals(w.Type, WorkoutWindow.TypeActivity, StringComparison.OrdinalIgnoreCase));
            MinMaxAvg? hr = activity?.HR?.FirstOrDefault();

            double? avgSpeed = activity?.Speed?.FirstOrDefault()?.Avg;
            if (avgSpeed is null && h.Distance is > 0 && h.Duration is > 0)
                avgSpeed = h.Distance / h.Duration;

            DeviceInfo? dev = log.Device ?? h.Device;

            var workout = new Workout
            {
                FileName = fileName,
                ImportedAt = DateTime.Now,
                ActivityType = h.ActivityType,
                ActivityName = ActivityTypes.Describe(h),
                StartTime = h.StartTime?.DateTime ?? DateTime.MinValue,
                UtcOffset = h.StartTime.HasValue ? FormatOffset(h.StartTime.Value.Offset) : null,
                Duration = h.Duration ?? 0,
                PauseDuration = h.PauseDuration,
                Distance = h.Distance ?? 0,
                Energy = h.Energy,
                StepCount = h.StepCount,
                Ascent = h.Ascent,
                Descent = h.Descent,
                AvgHeartRate = hr?.Avg is double a ? a * 60 : null,   // Гц -> уд/мин
                MaxHeartRate = hr?.Max is double m ? m * 60 : null,
                AvgSpeed = avgSpeed,
                DeviceName = dev?.Name,
                DeviceSerial = dev?.SerialNumber,
                DeviceSoftware = dev?.Info?.Software,
                AutoLapDistance = h.Settings?.AutoLap?.Distance
            };

            // Данные индивидуального варианта: круги (Windows типа Lap / Autolap) с дистанцией
            var lapWindows = (log.Windows ?? new List<WindowEntry>())
                .Where(w => w.Window != null && w.Window.IsLap && w.Window.Distance.HasValue)
                .OrderBy(w => w.Time ?? DateTimeOffset.MaxValue)
                .ToList();

            int number = 1;
            foreach (WindowEntry entry in lapWindows)
            {
                WorkoutWindow w = entry.Window!;
                workout.Laps.Add(new Lap
                {
                    Number = number++,
                    LapType = w.IsAutoLap ? Lap.TypeAuto : Lap.TypeManual,
                    EndTime = entry.Time?.DateTime,
                    Duration = w.Duration,
                    Distance = w.Distance!.Value,
                    AvgSpeed = w.Speed?.FirstOrDefault()?.Avg,
                    AvgHeartRate = w.HR?.FirstOrDefault()?.Avg is double lapHr ? lapHr * 60 : null
                });
            }

            return workout;
        }

        private static string FormatOffset(TimeSpan offset)
        {
            string sign = offset < TimeSpan.Zero ? "-" : "+";
            offset = offset.Duration();
            return $"{sign}{offset.Hours:D2}:{offset.Minutes:D2}";
        }
    }
}
