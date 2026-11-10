using System.Text.Json.Serialization;

namespace SuuntoLapDistance.Models
{
    /// <summary>
    /// Элемент массива "Windows". Каждое окно – это агрегированные показатели
    /// за некоторый интервал тренировки: круг (Lap), автокруг (Autolap),
    /// вся тренировка (Activity, Move).
    /// </summary>
    public class WindowEntry
    {
        [JsonPropertyName("Window")]
        public WorkoutWindow? Window { get; set; }

        /// <summary>Момент окончания интервала.</summary>
        [JsonPropertyName("TimeISO8601")]
        public DateTimeOffset? Time { get; set; }
    }

    /// <summary>Агрегированные показатели интервала (круга).</summary>
    public class WorkoutWindow
    {
        /// <summary>Типы окон, которые встречаются в файлах Suunto.</summary>
        public const string TypeLap = "Lap";          // круг, отмеченный вручную
        public const string TypeAutolap = "Autolap";  // автоматический круг (по дистанции/времени)
        public const string TypeActivity = "Activity";
        public const string TypeMove = "Move";

        [JsonPropertyName("ActivityId")]
        public int? ActivityId { get; set; }

        /// <summary>Тип окна: Lap, Autolap, Activity, Move.</summary>
        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        /// <summary>Дистанция интервала, м. (основной показатель варианта 9)</summary>
        [JsonPropertyName("Distance")]
        public double? Distance { get; set; }

        [JsonPropertyName("DistanceMax")]
        public double? DistanceMax { get; set; }

        /// <summary>Продолжительность интервала, с.</summary>
        [JsonPropertyName("Duration")]
        public double? Duration { get; set; }

        /// <summary>Энергозатраты, Дж.</summary>
        [JsonPropertyName("Energy")]
        public double? Energy { get; set; }

        [JsonPropertyName("Ascent")]
        public double? Ascent { get; set; }

        [JsonPropertyName("Descent")]
        public double? Descent { get; set; }

        /// <summary>Скорость, м/с (массив из одного элемента Avg/Max/Min).</summary>
        [JsonPropertyName("Speed")]
        public List<MinMaxAvg>? Speed { get; set; }

        /// <summary>Пульс, Гц (уд/с). Для перевода в уд/мин умножить на 60.</summary>
        [JsonPropertyName("HR")]
        public List<MinMaxAvg>? HR { get; set; }

        /// <summary>Каденс, Гц.</summary>
        [JsonPropertyName("Cadence")]
        public List<MinMaxAvg>? Cadence { get; set; }

        [JsonPropertyName("Altitude")]
        public List<MinMaxAvg>? Altitude { get; set; }

        /// <summary>Является ли окно кругом (ручным или автоматическим).</summary>
        [JsonIgnore]
        public bool IsLap =>
            string.Equals(Type, TypeLap, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(Type, TypeAutolap, StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsManualLap => string.Equals(Type, TypeLap, StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsAutoLap => string.Equals(Type, TypeAutolap, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Среднее / максимальное / минимальное значение показателя.</summary>
    public class MinMaxAvg
    {
        [JsonPropertyName("Avg")]
        public double? Avg { get; set; }

        [JsonPropertyName("Max")]
        public double? Max { get; set; }

        [JsonPropertyName("Min")]
        public double? Min { get; set; }
    }
}
