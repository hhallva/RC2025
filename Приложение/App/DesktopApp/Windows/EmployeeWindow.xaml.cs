using DataLayer.DataContexts;
using DataLayer.Models;
using DataLayer.Services;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public static AppDbContext _context = new();
        private EmployeeService _service = new(new HttpClient());
        private Employee _employee;

        public EmployeeWindow(Employee? employee = null, Department? department = null)
        {
            InitializeComponent();
            _employee = employee;

        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_employee != null)
                _employee = await _service.GetAsync(_employee.EmployeeId);
            else
                _employee = new();

            EventsDataGrid.ItemsSource = _employee.Events;
            DataContext = _employee;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            #region Проверки
            if (string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(PatronymicTextBox.Text))
            {
                MessageBox.Show("ФИО являются обязательными полями для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
            {
                MessageBox.Show("Поле \"Рабочий телефон\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Поле \"Email\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(DepartmentTextBox.Text))
            {
                MessageBox.Show("Поле \"Структурное подразделение\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(PositionTextBox.Text))
            {
                MessageBox.Show("Поле \"Должность\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
            {
                MessageBox.Show("Поле \"Кабинет\" обязательно для заполнения.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string pattern = @"^[\+]?[\d\(\)\-\s#]{1,20}$";
            if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
                {
                    MessageBox.Show("Поле \"Мобильный телефон\" заполненно неправильно.\nНомер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
            {
                MessageBox.Show("Поле \"Рабочий телефон\" заполненно неправильно.\nНомер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            pattern = "^[a-zA-Z0-9а-яА-Я]+@[a-zA-Z0-9а-яА-Я\\.]+\\.[a-zA-Zа-яА-Я0-9]{2,}$";
            if (!Regex.IsMatch(EmailTextBox.Text, pattern))
            {
                MessageBox.Show("Поле \"Email\" заполненно неправильно.\nЭлектронная почта должна быть написанна в соответствии с шаблоном: x@x.x\nx - символ русского или английского алфавита, почта может модержать числа.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            pattern = "^[a-zA-Z0-9а-яА-Я\\s]{1,10}$";
            if (!Regex.IsMatch(CabinetTextBox.Text, pattern))
            {
                MessageBox.Show("Поле \"Кабинет\" заполненно неправильно.\nКабинет может содежать только 10 символов.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            #endregion

            try
            {
                await _service.UpdateAsync(_employee);
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
                await _service.DismissAsync(_employee.EmployeeId);
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
