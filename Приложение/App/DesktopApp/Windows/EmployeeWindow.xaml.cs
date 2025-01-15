using DataLayer.DataContexts;
using DataLayer.Models;
using DataLayer.Services;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

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
        private List<Position> _positions;

        public EmployeeWindow(Employee? employee = null, Department? department = null)
        {
            InitializeComponent();
            _employee = employee;

        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _positions = await _positionService.GetAsync();
            PositionComboBox.ItemsSource = _positions;

            if (_employee != null)
                _employee = await _employeeService.GetAsync(_employee.EmployeeId);
            else
                _employee = new();

            EventsDataGrid.ItemsSource = _employee.Events;
            DataContext = _employee;
            PositionComboBox.SelectedItem = _employee.Position; //_positions.FirstOrDefault(p => p.PositionId == _employee.PositionId);


        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            //#region Проверки
            //StringBuilder errors = new();

            //if (string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text))
            //    errors.AppendLine("Поле \"Фамилия\" являются обязательным для заполнения.");
            //if (string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
            //    errors.AppendLine("Поле \"Рабочий телефон\" обязательно для заполнения.");
            //if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            //    errors.AppendLine("Поле \"Email\" обязательно для заполнения.");
        
            ////if (string.IsNullOrWhiteSpace(DepartmentComboBox.Text))
            ////{
            ////    MessageBox.Show("Поле \"Структурное подразделение\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            ////    return;
            ////}
            //if (string.IsNullOrWhiteSpace(DepartmentTextBox.Text))
            //{
            //    MessageBox.Show("Поле \"Структурное подразделение\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(PositionComboBox.Text))
            //{
            //    MessageBox.Show("Поле \"Должность\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
            //{
            //    MessageBox.Show("Поле \"Кабинет\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //string pattern = @"^[\+]?[\d\(\)\-\s#]{1,20}$";
            //if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            //{
            //    if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
            //    {
            //        MessageBox.Show("Поле \"Мобильный телефон\" заполненно неправильно.\nНомер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //        return;
            //    }
            //}
            //if (!Regex.IsMatch(WorkPhoneTextBox.Text, pattern))
            //{
            //    MessageBox.Show("Поле \"Рабочий телефон\" заполненно неправильно.\nНомер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //pattern = "^[a-zA-Z0-9а-яА-Я]+@[a-zA-Z0-9а-яА-Я\\.]+\\.[a-zA-Zа-яА-Я0-9]{2,}$";
            //if (!Regex.IsMatch(EmailTextBox.Text, pattern))
            //{
            //    MessageBox.Show("Поле \"Email\" заполненно неправильно.\nЭлектронная почта должна быть написанна в соответствии с шаблоном: x@x.x\nx - символ русского или английского алфавита, почта может модержать числа.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //pattern = "^[a-zA-Z0-9а-яА-Я\\s]{1,10}$";
            //if (!Regex.IsMatch(CabinetTextBox.Text, pattern))
            //{
            //    MessageBox.Show("Поле \"Кабинет\" заполненно неправильно.\nКабинет может содежать только 10 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //#endregion

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
    }
}
