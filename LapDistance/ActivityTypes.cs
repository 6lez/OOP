using SuuntoLapDistance.Models;

namespace SuuntoLapDistance.Services
{
    /// <summary>
    /// Справочник кодов видов активности, используемых в поле Header.ActivityType
    /// файлов часов Suunto.
    /// </summary>
    public static class ActivityTypes
    {
        private static readonly Dictionary<int, string> Names = new()
        {
            { 1, "Мультиспорт" },
            { 3, "Бег" },
            { 4, "Велоспорт" },
            { 5, "Горный велосипед" },
            { 6, "Катание на роликах/коньках" },
            { 7, "Аэробика" },
            { 8, "Йога / пилатес" },
            { 9, "Треккинг" },
            { 10, "Парусный спорт" },
            { 11, "Каякинг" },
            { 12, "Ходьба" },
            { 13, "Скандинавская ходьба" },
            { 14, "Ориентирование" },
            { 15, "Беговые лыжи" },
            { 16, "Горные лыжи" },
            { 17, "Сноуборд" },
            { 18, "Плавание" },
            { 34, "Бадминтон" },
        };

        /// <summary>Возвращает название вида активности по коду.</summary>
        public static string GetName(int? code)
        {
            if (code == null) return "Не указан";
            return Names.TryGetValue(code.Value, out var name)
                ? name
                : $"Активность (код {code.Value})";
        }

        /// <summary>
        /// Формирует отображаемое название вида активности из заголовка тренировки:
        /// название по коду + пользовательское имя режима, если оно задано.
        /// </summary>
        public static string Describe(WorkoutHeader header)
        {
            string name = GetName(header.ActivityType);

            // В файлах поле Activity может содержать строку "null" – игнорируем её
            string? custom = header.Activity?.Trim();
            if (!string.IsNullOrEmpty(custom) &&
                !string.Equals(custom, "null", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(custom, name, StringComparison.OrdinalIgnoreCase))
            {
                name += $" ({custom})";
            }

            return name;
        }
    }
}
