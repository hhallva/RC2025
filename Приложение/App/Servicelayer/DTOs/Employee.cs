using System.Drawing;

namespace DataLayer.Models
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name} {Patronymic}";

        public int? DayAfterDismissal => (DismissalDate == null) ?
         null : (int)(DateTime.Now - DismissalDate.Value.ToDateTime(TimeOnly.MinValue)).TotalDays;

        public bool DismissedAgo => (DismissalDate != null && DayAfterDismissal <= 30);
        public bool IsDismiss => (DismissalDate != null);

        //public override bool Equals(object? obj)
        //{
        //    if (obj is Employee employee)
        //        return employee.DirectManager == DirectManager;
        //    return false;
        //}
    }
}

