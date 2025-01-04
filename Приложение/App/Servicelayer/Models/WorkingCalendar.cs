namespace DataLayer.Models;

/// <summary>
/// Список дней исключений в производственном календаре
/// </summary>
public partial class WorkingCalendar
{
    /// <summary>
    /// Идентификатор строки
    /// </summary>
    public long WorkingCalendarId { get; set; }

    /// <summary>
    /// День-исключение
    /// </summary>
    public DateOnly ExceptionDate { get; set; }

    /// <summary>
    /// 0 - будний день, но законодательно принят выходным
    /// </summary>
    public bool IsWorkingDay { get; set; }
}
