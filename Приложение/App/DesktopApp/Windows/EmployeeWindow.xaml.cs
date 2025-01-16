using DataLayer.Models;
using DataLayer.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private EmployeeService _employeeService = new(new HttpClient());
        private PositionService _positionService = new(new HttpClient());
        private Employee _employee;
        private Department _department;
        private List<Position> _positions;
        //private List<Employee> _directManagers;

        public EmployeeWindow(Employee? employee = null, Department? department = null)
        {
            InitializeComponent();
            _employee = employee;
            _department = department;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _positions = await _positionService.GetAsync();

            if (_employee != null)
                _employee = await _employeeService.GetAsync(_employee.EmployeeId);
            else
                _employee = new();



            EventsListView.ItemsSource = _employee.Events;
            FreeDaysListView.ItemsSource = _employee.AbsenceEventEmployees
                .Where(e => e.AbsenceType != "Отпуск")
                .OrderBy(e => e.StartDate);
            HolidaysListView.ItemsSource = _employee.AbsenceEventEmployees
                .Where(e => e.AbsenceType == "Отпуск")
                .OrderBy(e => e.StartDate);

            DataContext = _employee;
            PositionComboBox.ItemsSource = _positions;
            PositionComboBox.SelectedItem = _employee.Position; //_positions.FirstOrDefault(p => p.PositionId == _employee.PositionId);

            //Попытка реализовать DirectManagerComboBox
            //if (_department != null)
            //    _directManagers = _department.Employees.Except(new List<Employee> { _employee }).ToList(); //Не удаляет

            //DirectManagerComboBox.ItemsSource = _directManagers;
            //DirectManagerComboBox.SelectedItem = _employee.DirectManager; 
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            #region Проверки
            //StringBuilder errors = new();

            //if (string.IsNullOrWhiteSpace(SurnameTextBox.Text))
            //    errors.AppendLine("Поле \"Фамилия\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            //    errors.AppendLine("Поле \"Имя\"  обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
            //    errors.AppendLine("Поле \"Рабочий телефон\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            //    errors.AppendLine("Поле \"Email\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(DepartmentTextBox.Text)) //DepartmentComboBox.Text
            //    errors.AppendLine("Поле \"Структурное подразделение\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(PositionComboBox.Text))
            //    errors.AppendLine("Поле \"Должность\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
            //    errors.AppendLine("Поле \"Кабинет\" обязательно для заполнения.");

            //string pattern = @"^[\+]?[\d\(\)\-\s#]{1,20}$";
            //if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            //    if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
            //        errors.AppendLine("Поле \"Мобильный телефон\" заполненно неправильно.");

            //if (!string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
            //    if (!Regex.IsMatch(WorkPhoneTextBox.Text, pattern))
            //        errors.AppendLine("Поле \"Рабочий телефон\" заполненно неправильно.");

            //if (errors.ToString().Contains("заполненно неправильно."))
            //    errors.AppendLine("Номер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.");

            //pattern = "^[a-zA-Z0-9а-яА-Я]+@[a-zA-Z0-9а-яА-Я\\.]+\\.[a-zA-Zа-яА-Я0-9]{2,}$";
            //if (!Regex.IsMatch(EmailTextBox.Text, pattern))
            //    errors.AppendLine("Поле \"Email\" заполненно неправильно.\nЭлектронная почта должна быть написанна в соответствии с шаблоном: x@x.x\nx - символ русского или английского алфавита, почта может модержать числа.");  

            //pattern = "^[a-zA-Z0-9а-яА-Я\\s]{1,10}$";
            //if (!Regex.IsMatch(CabinetTextBox.Text, pattern))
            //    errors.AppendLine("Поле \"Кабинет\" заполненно неправильно.\nКабинет может содежать только 10 символов.");

            //if(errors.Length > 0)
            //{
            //    MessageBox.Show(errors.ToString(), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            #endregion

            try
            {
                await _employeeService.UpdateAsync(_employee);
                MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении информации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            if (_employee.Events.Where(e => e.EventTypeId == 1)
                .Any(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now))
            {
                MessageBox.Show("Невозмонжно уволить сотрудника.\nПрисутсвтует запись на обучение.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите уволить сотрудника?", "Подтверждение увольнения", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            try
            {
                await _employeeService.DismissAsync(_employee.EmployeeId);
                MessageBox.Show("Сотрудник успешно уволен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при увольнении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            List<Event> filteredEvents = new List<Event>();
            bool pastChecked = PastCheckBox.IsChecked ?? false;
            bool presentChecked = NowCheckBox.IsChecked ?? false;
            bool futureChecked = FutureCheckBox.IsChecked ?? false;

            //Дописать
        }
    }
}
