using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LapDistance.Data
{
    /// <summary>
    /// Сущность базы данных: круг тренировки (окно Windows типа Lap / Autolap).
    /// Таблица Laps. Хранит данные, необходимые для варианта 9 (дистанция по кругам).
    /// </summary>
    public class Lap
    {
        public const string TypeManual = "Lap";
        public const string TypeAuto = "Autolap";

        [Key]
        public int Id { get; set; }

        /// <summary>Внешний ключ на тренировку.</summary>
        public int WorkoutId { get; set; }

        public Workout? Workout { get; set; }

        /// <summary>Порядковый номер круга в тренировке (по времени окончания).</summary>
        public int Number { get; set; }

        /// <summary>Тип круга: "Lap" (ручной) или "Autolap" (автокруг).</summary>
        [MaxLength(20)]
        public string LapType { get; set; } = TypeManual;

        /// <summary>Время окончания круга (локальное время часов).</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>Продолжительность круга, с.</summary>
        public double? Duration { get; set; }

        /// <summary>Дистанция круга, м — основной показатель варианта 9.</summary>
        public double Distance { get; set; }

        /// <summary>Средняя скорость на круге, м/с.</summary>
        public double? AvgSpeed { get; set; }

        /// <summary>Средний пульс на круге, уд/мин.</summary>
        public double? AvgHeartRate { get; set; }

        [NotMapped]
        public bool IsAutoLap => string.Equals(LapType, TypeAuto, StringComparison.OrdinalIgnoreCase);

        [NotMapped]
        public bool IsManualLap => !IsAutoLap;
    }
}
