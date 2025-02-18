using DataLayer.Models;
using DataLayer.Services;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private EmployeeService _employeeService;
        private PositionService _positionService = new(new HttpClient());
        private WorkingCalendarService _workingCalendarService = new(new HttpClient());
        private Employee _employee;
        private List<Department> _departments;
        private List<Position> _positions;
        private Random random = new();

        public Employee Employee { get => _employee; set => _employee = value; }

        public EmployeeWindow(Employee? employee = null, List<Department> departments = null, EmployeeService? employeeService = null)
        {
            InitializeComponent();
            Employee = employee;
            _departments = departments;
            _employeeService = employeeService ?? new(new HttpClient());
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Collapsed;
            DismissButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Collapsed;

            if (Employee != null)
            {
                EmployeeData.IsEnabled = false;
                EventData.IsEnabled = false;

                DismissButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
                Employee = await _employeeService.GetAsync(Employee.EmployeeId);
            }
            else
            {
                AddEventStackPanel.IsEnabled = false;

                AddButton.Visibility = Visibility.Visible;
                Employee = new();
            }

            DataContext = Employee;

            DepartmentComboBox.ItemsSource = _departments;
            DepartmentComboBox.SelectedItem = _departments.Find(e => e.DepartmentId == Employee.DepartmentId);

            _positions = await _positionService.GetAsync();
            PositionComboBox.ItemsSource = _positions;
            PositionComboBox.SelectedItem = Employee.Position;

            UpdateListViews();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (await CheckData())
                return;

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

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder password = new();
            while (password.Length < 8)
            {
                int type = random.Next(3);
                int charCode = type switch
                {
                    0 => random.Next((int)'A', (int)'Z' + 1),
                    1 => random.Next((int)'a', (int)'z' + 1),
                    _ => random.Next((int)'0', (int)'9' + 1)
                };

                password.Append((char)charCode);
            }
            Employee.Password = password.ToString();
            PasswordTextBox.Text = password.ToString();
        }

        private async void GenerateEmail_Click(object sender, RoutedEventArgs e)
        {
            var email = $"{Employee.Surname.ToLower()}@гкдр.ру";
            Employee.Email = email.ToString();
            EmailTextBox.Text = email.ToString();
        }

        private async Task<bool> CheckData()
        {
            StringBuilder errors = new();

            if (string.IsNullOrWhiteSpace(SurnameTextBox.Text))
                errors.AppendLine("Поле \"Фамилия\" обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                errors.AppendLine("Поле \"Имя\"  обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
                errors.AppendLine("Поле \"Рабочий телефон\" обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                errors.AppendLine("Поле \"Email\" обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(DepartmentComboBox.Text))
                errors.AppendLine("Поле \"Структурное подразделение\" обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(PositionComboBox.Text))
                errors.AppendLine("Поле \"Должность\" обязательно для заполнения.");
            if (string.IsNullOrWhiteSpace(CabinetTextBox.Text))
                errors.AppendLine("Поле \"Кабинет\" обязательно для заполнения.");

   
            string pattern = "^[\\+]?[\\d\\(\\)\\-\\s#]{1,20}$";
             pattern = @"^\+\d\(\d{3})\d{3}\-\d{2}-\d{2}$";
            if (!string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
                    errors.AppendLine("Поле \"Мобильный телефон\" заполненно неправильно.");

            if (!string.IsNullOrWhiteSpace(WorkPhoneTextBox.Text))
                if (!Regex.IsMatch(WorkPhoneTextBox.Text, pattern))
                    errors.AppendLine("Поле \"Рабочий телефон\" заполненно неправильно.");

            if (errors.ToString().Contains("заполненно неправильно."))
                errors.AppendLine("Номер телефона может содежать только цифры и спецсимволы \"+\", \"(\", \")\", \"-\", \" \", \"#\"\nМаксимум 20 символов.");

            pattern = "^[e-zA-Z0-9а-яА-Я]+@[e-zA-Z0-9а-яА-Я\\.]+\\.[e-zA-Zа-яА-Я0-9]{2,}$";
            if (!string.IsNullOrWhiteSpace(EmailTextBox.Text))
                if (!Regex.IsMatch(EmailTextBox.Text, pattern))
                errors.AppendLine("Поле \"Email\" заполненно неправильно.\nЭлектронная почта должна быть написанна в соответствии с шаблоном: x@x.x\nx - символ русского или английского алфавита, почта может модержать числа.");

            var emlpoyees = await _employeeService.GetAllAsync();

            if (emlpoyees.Any(e => e.Email == Employee.Email))
               errors.AppendLine("Такая почта уже существует!");

            pattern = "^[e-zA-Z0-9а-яА-Я\\s]{1,10}$";
            if (!string.IsNullOrWhiteSpace(CabinetTextBox.Text))
                if (!Regex.IsMatch(CabinetTextBox.Text, pattern))
                errors.AppendLine("Поле \"Кабинет\" заполненно неправильно.\nКабинет может содежать только 10 символов.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        private async void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee.Events.Where(e => e.TypeId == 1)
                .Any(e => e.StartDate > DateTime.Now))
            {
                MessageBox.Show("Невозможно уволить сотрудника.\n Присутствует запись на обучение.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void FilterToggleButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateListViews();
        }

        private void UpdateListViews()
        {
            bool showFuture = FutureToggleButton.IsChecked == true;
            bool showCurrent = CurrentToggleButton.IsChecked == true;
            bool showPast = PastToggleButton.IsChecked == true;

            var now = DateTime.Now.Date;

            EventsListView.ItemsSource = Employee.Events
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .OrderBy(e => e.StartDate);

            FreeDaysListView.ItemsSource = Employee.AbsenceEventEmployees
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .Where(e => !e.Type.ToLower().Contains("отпуск"))
                .OrderBy(e => e.StartDate);

            HolidaysListView.ItemsSource = Employee.AbsenceEventEmployees
                .Where(e => (showFuture && e.StartDate > now) || (showCurrent && e.StartDate <= now && e.EndDate >= now) || (showPast && e.EndDate < now))
                .Where(e => e.Type.ToLower().Contains("отпуск"))
                .OrderBy(e => e.StartDate);
        }

        private async void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (await CheckData())
                return;

            try
            {
                EditData(Employee);
                await _employeeService.AddEmployeeAsync(Employee);
                MessageBox.Show("Информация добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении информации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditData(Employee employee)
        {
            employee.Password = (string.IsNullOrWhiteSpace(employee.Password)) ? null : employee.Password.Trim();
            employee.Patronymic = (string.IsNullOrWhiteSpace(employee.Patronymic)) ? null : employee.Patronymic.Trim();
            employee.Phone = (string.IsNullOrWhiteSpace(employee.Phone)) ? null : employee.Phone.Trim();

            employee.PositionId = (PositionComboBox.SelectedItem as Position).PositionId;
            employee.DepartmentId = (DepartmentComboBox.SelectedItem as Department).DepartmentId;
        }

        private async void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new();
            if (TypeComboBox.SelectedIndex < 0)
                errors.AppendLine("Поле \"Вид события\" обязательно для заполнения.");
            if (FromDatePicker.SelectedDate is not DateTime)
                errors.AppendLine("Поле \"От:\" обязательно для заполнения.");
            if (ToDatePicker.SelectedDate is not DateTime)
                errors.AppendLine("Поле \"До:\" обязательно для заполнения.");
            if (FromDatePicker.SelectedDate > ToDatePicker.SelectedDate)
                errors.AppendLine("Дата начала должна быть раньше даты конца.");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var startDate = FromDatePicker.SelectedDate.Value;
                var endDate = ToDatePicker.SelectedDate.Value;
                var content = (TypeComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                var specialDays = await _workingCalendarService.GetAllAsync();

                if (content == "Обучение")
                {
                    Event education = new()
                    {
                        Name = "Обучение",
                        StartDate = startDate,
                        EndDate = endDate,
                        TypeId = 1,
                        Description = (string.IsNullOrWhiteSpace(DescriptionTextBox.Text)) ? null : DescriptionTextBox.Text,
                        Status = "Запланировано"
                    };

                    if (HasConflicts(education, Employee.AbsenceEventEmployees.ToList(), Employee.Events.ToList(), specialDays))
                        return;

                    await _employeeService.AddEventAsync(Employee.EmployeeId, education);
                    MessageBox.Show("Информация добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Employee.Events.Add(education);
                }
                else
                {
                    AbsenceEvent absenceEvent = new()
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        Type = content,
                        Description = (string.IsNullOrWhiteSpace(DescriptionTextBox.Text)) ? null : DescriptionTextBox.Text,
                        EmployeeId = Employee.EmployeeId
                    };

                    if (HasConflicts(absenceEvent, Employee.AbsenceEventEmployees.ToList(), Employee.Events.ToList(), specialDays))
                        return;

                    await _employeeService.AddAbsenseEventAsync(absenceEvent);
                    MessageBox.Show("Информация добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    Employee.AbsenceEventEmployees.Add(absenceEvent);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении информации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool HasConflicts(object newEvent, List<AbsenceEvent> absenceEvents, List<Event> events, List<WorkingCalendar> specialDays)
        {
            StringBuilder errors = new();
            if (newEvent is AbsenceEvent absence)
            {
                if (absenceEvents.Any(a => (absence.StartDate <= a.EndDate && a.StartDate <= absence.EndDate)))
                    errors.Append("Отпуск и отгул не могут быть в один одни даты (не могут пересекаться).");
                if (events.Any(e => (absence.StartDate <= e.EndDate && e.StartDate <= absence.EndDate) &&
                                (absence.Type == "Отгул" && e.TypeId == 1)))
                    errors.Append("Отгул и обучение не могут быть в одни даты (не могут пересекаться).");
                if (specialDays.Any(s => (absence.StartDate <= s.ExceptionDate && absence.EndDate >= s.ExceptionDate) &&
                                  (absence.Type == "Отгул" && s.IsWorkingDay == false)))
                    errors.Append("Отгул не может быть в выходной день по производственному календарю.");
            }
            if (newEvent is Event training)
            {
                if (absenceEvents.Any(a => (training.StartDate <= a.EndDate && a.StartDate <= training.EndDate) &&
                                               a.Type == "Отгул"))
                    errors.Append("Отгул и обучение не могут быть в одни даты (не могут пересекаться).");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        private void EditCheckBox_Click(object sender, RoutedEventArgs e)
        {
            EmployeeData.IsEnabled = EditCheckBox.IsChecked == true;    
            EventData.IsEnabled = EditCheckBox.IsChecked == true;
        }

       
    }
}