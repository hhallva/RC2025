using DataLayer.DTOs;
using DataLayer.Models;

namespace WebApp.Calendar
{
    public class CalendarViewModel()
    {
        public List<WorkingCalendar> ExeptionDays;
        public List<Employee?> Employees;
        public List<EventDto?> Events;

        public CalendarViewModel(List<WorkingCalendar> exeptionDays, List<Employee?> employees, List<EventDto?> events) : this()
        {
            ExeptionDays = exeptionDays;
            Employees = employees;
            Events = events;
        }

        public int Year { get; } = CalendarHelper.Date.Year;
        public int Month { get; } = CalendarHelper.Date.Month;

        public int DaysInMonth => DateTime.DaysInMonth(Year, Month);
        public int StartWeekDay
           => CalendarHelper.Date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)CalendarHelper.Date.DayOfWeek;
        public override string ToString() => CalendarHelper.Date.ToString("MMMM yyyy");

    }
}