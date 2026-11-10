using System.Text.Json;
using SuuntoLapDistance.Models;

namespace SuuntoLapDistance.Services
{
    /// <summary>
    /// Исключение, означающее, что файл прочитан, но его содержимое
    /// не является корректным файлом тренировки Suunto.
    /// </summary>
    public class InvalidWorkoutFileException : Exception
    {
        public InvalidWorkoutFileException(string message) : base(message) { }
        public InvalidWorkoutFileException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Чтение JSON-файла тренировки и его десериализация в объекты модели
    /// с помощью System.Text.Json (JsonSerializer).
    /// </summary>
    public static class WorkoutReader
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            // В файлах встречаются числа в виде строк и запятые в конце – разрешаем их
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        /// <summary>
        /// Загружает тренировку из файла.
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл не найден.</exception>
        /// <exception cref="IOException">Ошибка чтения файла.</exception>
        /// <exception cref="UnauthorizedAccessException">Нет доступа к файлу.</exception>
        /// <exception cref="JsonException">Содержимое не является корректным JSON.</exception>
        /// <exception cref="InvalidWorkoutFileException">JSON корректен, но не содержит данных тренировки.</exception>
        public static SuuntoWorkoutFile Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Не указан путь к файлу.", nameof(path));

            // 1. Чтение содержимого файла (класс File)
            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidWorkoutFileException("Файл пуст.");

            // 2. Десериализация JSON -> объекты C#
            SuuntoWorkoutFile? workout = JsonSerializer.Deserialize<SuuntoWorkoutFile>(json, Options);

            // 3. Проверка, что это действительно файл тренировки Suunto
            if (workout?.DeviceLog == null)
                throw new InvalidWorkoutFileException(
                    "Файл не содержит раздел \"DeviceLog\" – это не файл тренировки Suunto.");

            if (workout.DeviceLog.Header == null)
                throw new InvalidWorkoutFileException(
                    "В файле отсутствует раздел \"Header\" с общей информацией о тренировке.");

            return workout;
        }
    }
}
