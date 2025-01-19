namespace DataLayer.Models
{
    public partial class AbsenceEvent
    {
        public DateOnly EndDate => StartDate.AddDays(DaysCount - 1);
    }
}



