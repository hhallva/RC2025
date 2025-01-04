namespace ServiceLayer.Models
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name} {Patronymic}";
    }
}
