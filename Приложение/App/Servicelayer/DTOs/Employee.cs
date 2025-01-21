﻿namespace DataLayer.Models
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name} {Patronymic}";

        public int? DayAfterDismissal => (DismissalDate == null) ?
         null : (int)(DateTime.Now.Date - DismissalDate.Value.Date).TotalDays;

        public bool DismissedAgo => (DismissalDate != null && DayAfterDismissal <= 30);

        public bool IsDismiss => (DismissalDate != null);
    }
}

