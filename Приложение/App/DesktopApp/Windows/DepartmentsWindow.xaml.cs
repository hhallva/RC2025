using DataLayer.DataContexts;
using DataLayer.Models;
using DataLayer.Services;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for DepartmentsWindow.xaml
    /// </summary>
    public partial class DepartmentsWindow : Window
    {
        private DepartmentService _departmentService = new(new AppDbContext());
        private readonly EmployeeService _employeeService = new EmployeeService();
        List<Employee> employees = new();
        List<Department> _departments;

        public DepartmentsWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDepartmentsAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            _departments = (List<Department>)await _departmentService.GetDepartmentsAsync();
            ShowDepartments(_departments);
        }

        private void ShowDepartments(IEnumerable<Department>? departments)
        {
            Department company = new() { Name = "Дороги России" };
            departments = departments
                .Where(d => d.ParentDepartmentId == null)
                .OrderBy(d => int.Parse(d.DepartmentId));
            foreach (var department in departments)
                company.ChildDepartment.Add(department);
            departmentsTreeView.Items.Add(company);
        }

        private void DepartmentsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            employeesListView.ItemsSource = null;
            if (departmentsTreeView.SelectedItem is not Department department)
                return;

            employees.Clear();
            FillEmployeesList(department);
            employeesListView.ItemsSource = employees.OrderBy(e => e.FullName);
        }

        private void FillEmployeesList(Department department)
        {
            employees.AddRange(department.Employees
                .Where(e => e.DismissalDate == null || e.DayAfterDismissal <= 30));
            foreach (var childDepartment in department.ChildDepartment)
                FillEmployeesList(childDepartment);
        }

        private void EmployeesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (departmentsTreeView.SelectedItem is not Department department)
                return;
            if (employeesListView.SelectedItem is not Employee employee)
                return;
            if (employee.DismissalDate != null)
                return;

            EmployeeWindow employeeWindow = new(employee, _departments, _employeeService);
            employeeWindow.ShowDialog();

            FillEmployeesList(department);
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsTreeView.SelectedItem is not Department)
                return;

            EmployeeWindow employeeWindow = new(null, _departments);
            employeeWindow.ShowDialog();
        }
    }
}