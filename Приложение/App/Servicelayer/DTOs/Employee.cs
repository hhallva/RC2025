namespace DataLayer.Models
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name} {Patronymic}";

        public bool IsDismiss => (DismissalDate != null);

        public int? DayAfterDismissal => (!IsDismiss) ?
                    null : 
                    (int)(DateTime.Now.Date - DismissalDate.Value.Date).TotalDays;

        public bool IsDismissedAgo => (IsDismiss && DayAfterDismissal > 30);

    }
}

