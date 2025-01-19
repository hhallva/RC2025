using DataLayer.DataContexts;
using DataLayer.Models;
using DataLayer.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
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
            _departments = await _departmentService.GetDepartmentsAsync();
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
            employeesListView.ItemsSource = new ObservableCollection<Employee>(employees.OrderBy(e => e.FullName));
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

            var currentItem = departmentsTreeView.SelectedItem;

            EmployeeWindow employeeWindow = new(employee, department, _employeeService);
            employeeWindow.ShowDialog();

            UpdateTreeView();
            FillEmployeesList(department);

            //var dep = _departments.FirstOrDefault(d => d.DepartmentId == department.DepartmentId);

            //var tvi = departmentsTreeView
            //    .ItemContainerGenerator
            //    .ContainerFromItem(dep) as TreeViewItem;
            //if (tvi != null)
            //{
            //    tvi.IsSelected = true;
            //}
        }

        private async void UpdateTreeView()
        {
            departmentsTreeView.Items.Clear();
            _departments = await _departmentService.GetDepartmentsAsync();
            AddRootNode(_departments);
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsTreeView.SelectedItem is not TreeViewItem item ||
               item.DataContext is not Department department)
                return;

            EmployeeWindow employeeWindow = new(null, department);
            employeeWindow.ShowDialog();
        }
    }
}