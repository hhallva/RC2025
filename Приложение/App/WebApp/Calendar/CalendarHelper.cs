namespace WebApp.Calendar
{
    public static class CalendarHelper
    {
        public static DateTime Date { get; set; } = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public static void SetCurrentMonth() => Date = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        // для переключения между месяцами
        public static void AddMonths(int month) => Date = Date.AddMonths(month);
    }

}
