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

            SurnameTextBox.Text = _employee.Surname;
            NameTextBox.Text = _employee.Name;
            PatronymicTextBox.Text = _employee.Patronymic;
            BirthdayTextBox.Text = _employee.Birthday.ToString();
            PhoneTextBox.Text = _employee.Phone;
            WorkPhoneTextBox.Text = _employee.WorkPhone;
            EmailTextBox.Text = _employee.Email;
            DepartmentTextBox.Text = _employee.Department.Name;
            PositionTextBox.Text = _employee.Position.Name;
            CabinetTextBox.Text = _employee.Cabinet;

            DirectManagerComboBox.Items.Add(_employee.FullName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
