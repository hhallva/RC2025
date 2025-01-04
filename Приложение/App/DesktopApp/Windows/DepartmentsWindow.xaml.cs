using ServiceLayer.DataContexts;
using ServiceLayer.Models;
using ServiceLayer.Services;
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
        List<Employee> employees = new();

        public DepartmentsWindow()
        {
            InitializeComponent();
        }

        #region Events
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var departments = await _service.GetDepartmentsAsync();
            AddRootNode(departments);
        }

        private void DepartmentsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            employeesListView.ItemsSource = null;
            if (departmentsTreeView.SelectedItem is not TreeViewItem item ||
                item.DataContext is not Department department)
                return;

            employees.Clear();
            FillEmployeesList(department);
            employeesListView.ItemsSource = employees;
        }

        private void EmployeesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (employeesListView.SelectedItem is not Employee employee)
                return;

            EmployeeWindow employeeWindow = new(employee);
            employeeWindow.ShowDialog();
        }
        #endregion


        private void FillEmployeesList(Department department)
        {
            employees.AddRange(department.Employees);
            foreach (var childDepartment in department.InverseParentDepartment)
                FillEmployeesList(childDepartment);
        }

        private void AddRootNode(IEnumerable<Department> departments)
        {
            // добавили в treeView название компании (главный узел)
            TreeViewItem rootItem = new() { Header = "Дороги России" };
            departmentsTreeView.Items.Add(rootItem);

            // получили список основных отделов компании и добавили их
            departments = departments
                .Where(d => d.ParentDepartmentId == null)
                .OrderBy(d => int.Parse(d.DepartmentId));
            FillDepartmentsTreeView(departments, rootItem);
        }
        private void FillDepartmentsTreeView(IEnumerable<Department> departments, TreeViewItem currentItem)
        {
            foreach (var department in departments)
            {
                // здесь department.DepartmentId для проверки. нужно написать: Header = department.Name
                TreeViewItem childItem = new()
                {
                    Header = department.Name,
                    DataContext = department
                };
                currentItem.Items.Add(childItem);

                // добавляем дочерние отделы для текущего
                FillDepartmentsTreeView(department.InverseParentDepartment.OrderBy(d => d.DepartmentId), childItem);
            }
        }

      
    }
}