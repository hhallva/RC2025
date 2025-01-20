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
        IEnumerable<Department> _departments;
        List<Employee> employees = new();

        private readonly EmployeeService _employeeService = new EmployeeService();

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
            var departments = await _departmentService.GetDepartmentsAsync();
            ShowDepartments(departments);
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

            EmployeeWindow employeeWindow = new(employee, department, _employeeService);
            employeeWindow.ShowDialog();

            FillEmployeesList(department);
        }

        //private async void UpdateTreeView()
        //{
        //    departmentsTreeView.Items.Clear();
        //    _departments = await _departmentService.GetDepartmentsAsync();
        //}

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsTreeView.SelectedItem is not Department department)
                return;

            EmployeeWindow employeeWindow = new(null, department);
            employeeWindow.ShowDialog();
        }
    }
}