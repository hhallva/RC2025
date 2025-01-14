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
        private DepartmentService _service = new(new AppDbContext());
        IEnumerable<Department> _departments;
        List<Employee> employees = new();

        public DepartmentsWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _departments = await _service.GetDepartmentsAsync();
            AddRootNode(_departments);
        }

        private void AddRootNode(IEnumerable<Department> departments)
        {
            TreeViewItem rootItem = new() { Header = "Дороги России" };
            departmentsTreeView.Items.Add(rootItem);

            departments = departments
                .Where(d => d.ParentDepartmentId == null)
                .OrderBy(d => int.Parse(d.DepartmentId));
            FillDepartmentsTreeView(departments, rootItem);
        }

        private void FillDepartmentsTreeView(IEnumerable<Department> departments, TreeViewItem currentItem)
        {
            foreach (var department in departments)
            {
                TreeViewItem childItem = new()
                {
                    Header = department.Name,
                    DataContext = department
                };
                currentItem.Items.Add(childItem);

                FillDepartmentsTreeView(department.InverseParentDepartment.OrderBy(d => d.DepartmentId), childItem);
            }
        }

        private void DepartmentsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            employeesListView.ItemsSource = null;
            if (departmentsTreeView.SelectedItem is not TreeViewItem item ||
                item.DataContext is not Department department)
                return;

            employees.Clear();
            FillEmployeesList(department);
            employeesListView.ItemsSource = employees.OrderBy(e => e.FullName);
        }

        private void FillEmployeesList(Department department)
        {
            employees.AddRange(department.Employees
                .Where(e => e.DismissalDate == null || e.DayAfterDismissal <= 30));
            foreach (var childDepartment in department.InverseParentDepartment)
                FillEmployeesList(childDepartment);
        }

        private void EmployeesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (departmentsTreeView.SelectedItem is not TreeViewItem item ||
               item.DataContext is not Department department)
                return;
            if (employeesListView.SelectedItem is not Employee employee)
                return;
            if (employee.DismissalDate != null)
                return;

            EmployeeWindow employeeWindow = new(employee);
            employeeWindow.ShowDialog();

            UpdateTreeView();
            FillEmployeesList(department);
        }

        private async void UpdateTreeView()
        {
            departmentsTreeView.Items.Clear();
            _departments = await _service.GetDepartmentsAsync();
            AddRootNode(_departments);
        }
    }
}