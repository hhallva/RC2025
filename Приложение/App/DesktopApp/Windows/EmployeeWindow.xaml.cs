using DataLayer.Models;
using System.Windows;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private Employee _employee;

        public EmployeeWindow(Employee? employee = null)
        {   
            InitializeComponent();

            _employee = employee ?? new();
            Title = _employee.FullName;
        }
    }
}
