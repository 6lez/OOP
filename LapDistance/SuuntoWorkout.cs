using System.Text.Json.Serialization;

namespace SuuntoLapDistance.Models
{
    // ------------------------------------------------------------------
    //  Классы модели данных, соответствующие структуре JSON-файла
    //  тренировки, экспортированного из часов Suunto.
    //
    //  Структура файла (сокращённо):
    //  {
    //    "DeviceLog": {
    //      "Header":  { ... общая информация о тренировке ... },
    //      "Samples": [ ... измерения (не используются в варианте 9) ... ],
    //      "Windows": [ { "Window": { ... агрегированные показатели круга ... },
    //                     "TimeISO8601": "..." }, ... ],
    //      "Device":  { ... информация об устройстве ... }
    //    }
    //  }
    //
    //  Свойства JSON, для которых в классах нет соответствующих членов,
    //  JsonSerializer просто пропускает, поэтому описаны только те
    //  данные, которые реально используются приложением.
    // ------------------------------------------------------------------

    /// <summary>Корневой объект JSON-файла.</summary>
    public class SuuntoWorkoutFile
    {
        [JsonPropertyName("DeviceLog")]
        public DeviceLog? DeviceLog { get; set; }
    }

    /// <summary>Журнал устройства: заголовок, окна (круги) и информация о часах.</summary>
    public class DeviceLog
    {
        [JsonPropertyName("Header")]
        public WorkoutHeader? Header { get; set; }

        [JsonPropertyName("Windows")]
        public List<WindowEntry>? Windows { get; set; }

        [JsonPropertyName("Device")]
        public DeviceInfo? Device { get; set; }
    }

    /// <summary>Общая (сводная) информация о тренировке.</summary>
    public class WorkoutHeader
    {
        /// <summary>Название режима/активности (в файлах может быть строка "null").</summary>
        [JsonPropertyName("Activity")]
        public string? Activity { get; set; }

        /// <summary>Код вида активности Suunto (3 – бег, 12 – ходьба, 34 – бадминтон ...).</summary>
        [JsonPropertyName("ActivityType")]
        public int? ActivityType { get; set; }

        /// <summary>Дата и время начала тренировки.</summary>
        [JsonPropertyName("DateTime")]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>Продолжительность, с.</summary>
        [JsonPropertyName("Duration")]
        public double? Duration { get; set; }

        /// <summary>Длительность пауз, с.</summary>
        [JsonPropertyName("PauseDuration")]
        public double? PauseDuration { get; set; }

        /// <summary>Дистанция, м.</summary>
        [JsonPropertyName("Distance")]
        public double? Distance { get; set; }

        /// <summary>Энергозатраты, Дж.</summary>
        [JsonPropertyName("Energy")]
        public double? Energy { get; set; }

        /// <summary>Количество шагов.</summary>
        [JsonPropertyName("StepCount")]
        public int? StepCount { get; set; }

        /// <summary>Суммарный набор высоты, м.</summary>
        [JsonPropertyName("Ascent")]
        public double? Ascent { get; set; }

        /// <summary>Суммарный сброс высоты, м.</summary>
        [JsonPropertyName("Descent")]
        public double? Descent { get; set; }

        [JsonPropertyName("Altitude")]
        public AltitudeRange? Altitude { get; set; }

        [JsonPropertyName("HrZones")]
        public HrZones? HrZones { get; set; }

        [JsonPropertyName("EPOC")]
        public double? Epoc { get; set; }

        [JsonPropertyName("PeakTrainingEffect")]
        public double? PeakTrainingEffect { get; set; }

        /// <summary>Рекомендуемое время восстановления, с.</summary>
        [JsonPropertyName("RecoveryTime")]
        public double? RecoveryTime { get; set; }

        [JsonPropertyName("Settings")]
        public WorkoutSettings? Settings { get; set; }

        [JsonPropertyName("Device")]
        public DeviceInfo? Device { get; set; }
    }

    /// <summary>Диапазон высот за тренировку, м.</summary>
    public class AltitudeRange
    {
        [JsonPropertyName("Max")]
        public double? Max { get; set; }

        [JsonPropertyName("Min")]
        public double? Min { get; set; }
    }

    /// <summary>Время (с) в пульсовых зонах и их нижние границы (Гц).</summary>
    public class HrZones
    {
        [JsonPropertyName("Zone1Duration")] public double? Zone1Duration { get; set; }
        [JsonPropertyName("Zone2Duration")] public double? Zone2Duration { get; set; }
        [JsonPropertyName("Zone3Duration")] public double? Zone3Duration { get; set; }
        [JsonPropertyName("Zone4Duration")] public double? Zone4Duration { get; set; }
        [JsonPropertyName("Zone5Duration")] public double? Zone5Duration { get; set; }
        [JsonPropertyName("Zone2LowerLimit")] public double? Zone2LowerLimit { get; set; }
        [JsonPropertyName("Zone3LowerLimit")] public double? Zone3LowerLimit { get; set; }
        [JsonPropertyName("Zone4LowerLimit")] public double? Zone4LowerLimit { get; set; }
        [JsonPropertyName("Zone5LowerLimit")] public double? Zone5LowerLimit { get; set; }
    }

    /// <summary>Настройки записи тренировки.</summary>
    public class WorkoutSettings
    {
        [JsonPropertyName("AutoLap")]
        public AutoLapSettings? AutoLap { get; set; }
    }

    /// <summary>Настройки автокругов.</summary>
    public class AutoLapSettings
    {
        /// <summary>Дистанция автокруга, м.</summary>
        [JsonPropertyName("Distance")]
        public double? Distance { get; set; }

        /// <summary>Длительность автокруга, с.</summary>
        [JsonPropertyName("Duration")]
        public double? Duration { get; set; }

        [JsonPropertyName("Enabled")]
        public bool? Enabled { get; set; }
    }

    /// <summary>Информация об устройстве (часах).</summary>
    public class DeviceInfo
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("SerialNumber")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("Info")]
        public DeviceHardwareInfo? Info { get; set; }
    }

    public class DeviceHardwareInfo
    {
        [JsonPropertyName("HW")]
        public string? Hardware { get; set; }

        [JsonPropertyName("SW")]
        public string? Software { get; set; }
    }
}

