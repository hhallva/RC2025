using DataLayer.DataContexts;
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
