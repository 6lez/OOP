using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LapDistance.Data
{
    /// <summary>
    /// Сущность базы данных: тренировка (общая информация из Header JSON-файла).
    /// Таблица Workouts.
    /// </summary>
    public class Workout
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Имя исходного JSON-файла.</summary>
        [MaxLength(260)]
        public string FileName { get; set; } = "";

        /// <summary>Когда тренировка была импортирована в базу.</summary>
        public DateTime ImportedAt { get; set; }

        /// <summary>Код вида активности Suunto (3 – бег, 12 – ходьба, 34 – бадминтон ...).</summary>
        public int? ActivityType { get; set; }

        /// <summary>Название вида активности.</summary>
        [MaxLength(100)]
        public string ActivityName { get; set; } = "";

        /// <summary>Дата и время начала (локальное время часов).</summary>
        public DateTime StartTime { get; set; }

        /// <summary>Смещение часового пояса, например "+05:00".</summary>
        [MaxLength(6)]
        public string? UtcOffset { get; set; }

        /// <summary>Продолжительность, с.</summary>
        public double Duration { get; set; }

        /// <summary>Длительность пауз, с.</summary>
        public double? PauseDuration { get; set; }

        /// <summary>Дистанция, м.</summary>
        public double Distance { get; set; }

        /// <summary>Энергозатраты, Дж.</summary>
        public double? Energy { get; set; }

        public int? StepCount { get; set; }

        /// <summary>Набор высоты, м.</summary>
        public double? Ascent { get; set; }

        /// <summary>Сброс высоты, м.</summary>
        public double? Descent { get; set; }

        /// <summary>Средний пульс, уд/мин.</summary>
        public double? AvgHeartRate { get; set; }

        /// <summary>Максимальный пульс, уд/мин.</summary>
        public double? MaxHeartRate { get; set; }

        /// <summary>Средняя скорость, м/с.</summary>
        public double? AvgSpeed { get; set; }

        [MaxLength(100)]
        public string? DeviceName { get; set; }

        [MaxLength(50)]
        public string? DeviceSerial { get; set; }

        [MaxLength(50)]
        public string? DeviceSoftware { get; set; }

        /// <summary>Настроенная дистанция автокруга, м (нужна для пометки неполных автокругов).</summary>
        public double? AutoLapDistance { get; set; }

        /// <summary>Круги тренировки (данные индивидуального варианта 9).</summary>
        public List<Lap> Laps { get; set; } = new();

        /// <summary>Подпись для выпадающего списка сохранённых тренировок.</summary>
        [NotMapped]
        public string DisplayName =>
            $"{StartTime:dd.MM.yyyy HH:mm}  —  {ActivityName}, {Distance:N0} м, {Laps.Count} кругов  ({FileName})";
    }
}
