﻿using System.Drawing;

namespace DataLayer.Models
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name} {Patronymic}";

        public int? DayAfterDismissal => (DismissalDate == null) ?
         null : (int)(DateTime.Now - DismissalDate.Value.ToDateTime(TimeOnly.MinValue)).TotalDays;

        public bool DismissedAgo => (DismissalDate != null && DayAfterDismissal <= 30) ? false : true;
        public bool IsDismiss => (DismissalDate != null) ? true :  false;
    }
}

