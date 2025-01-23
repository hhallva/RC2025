namespace DataLayer.Models
{
    public partial class AbsenceEvent
    {
        public DateTime EndDate => StartDate.AddDays(DaysCount - 1);
    }
}



