using DataLayer.Models;
using DataLayer.Services;
using System.Net.Http;
using System.Windows;


namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private EmployeeService _employeeService;
        private PositionService _positionService = new(new HttpClient());
        private Employee _employee;
        private Department _department;
        private List<Position> _positions;

        public Employee Employee { get => _employee; set => _employee = value; }

        public EmployeeWindow(Employee? employee = null, Department? department = null, EmployeeService? employeeService = null)
        {
            InitializeComponent();
            Employee = employee;
            _department = department;
            _employeeService = employeeService ?? new(new HttpClient());
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _positions = await _positionService.GetAsync();

            if (Employee != null)
            {
                AddButton.Visibility = Visibility.Collapsed;
                Employee = await _employeeService.GetAsync(Employee.EmployeeId);
            }
            else
            {
                SaveButton.Visibility = Visibility.Collapsed;
                Employee = new();
            }



            UpdateListViews();
            DataContext = Employee;
            PositionComboBox.ItemsSource = _positions;
            PositionComboBox.SelectedItem = Employee.Position;
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
                EditData(Employee);
                await _employeeService.UpdateAsync(Employee);
                MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Employee.Position = PositionComboBox.SelectedItem as Position;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении информации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee.Events.Where(e => e.EventTypeId == 1)
                .Any(e => e.StartDate > DateTime.Now))
            {
                MessageBox.Show("Невозмонжно уволить сотрудника.\nПрисутсвтует запись на обучение.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите уволить сотрудника?", "Подтверждение увольнения", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            try
            {
                await _employeeService.DismissAsync(Employee.EmployeeId);
                MessageBox.Show("Сотрудник успешно уволен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при увольнении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateListViews();
        }

        private void UpdateListViews()
        {
            bool showFuture = FutureCheckBox.IsChecked == true;
            bool showCurrent = CurrentCheckBox.IsChecked == true;
            bool showPast = PastCheckBox.IsChecked == true;

            var now = DateTime.Now.Date;

            EventsListView.ItemsSource = Employee.Events
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .OrderBy(e => e.StartDate);

            FreeDaysListView.ItemsSource = Employee.AbsenceEventEmployees
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .Where(e => !e.AbsenceType.ToLower().Contains("отпуск"))
                .OrderBy(e => e.StartDate);

            HolidaysListView.ItemsSource = Employee.AbsenceEventEmployees
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .Where(e => e.AbsenceType.ToLower().Contains("отпуск"))
                .OrderBy(e => e.StartDate);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditData(Employee);
                await _employeeService.AddAsync(Employee);
                MessageBox.Show("Информаия добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении информации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditData(Employee employee)
        {
            employee.Password = employee.Password ?? "Password";
            employee.AbsenceEventEmployees.Clear();
            employee.PositionId = (PositionComboBox.SelectedItem as Position).PositionId;
            employee.Position = (PositionComboBox.SelectedItem as Position);
        }
    }
}

