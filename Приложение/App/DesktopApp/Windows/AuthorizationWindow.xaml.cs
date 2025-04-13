using DataLayer.DataContexts;
using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DesktopApp.Windows
{
    public partial class AuthorizationWindow : Window
    {
        public List<Employee> Employees { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();

            AppDbContext dbContext = new();

            var image = dbContext.Photos.SingleOrDefault(p => p.Id == 2);

            if (image.FileData != null)
                ProfilePhoto.Source = BitmapFrame.Create(new MemoryStream(image.FileData));
            else
                ProfilePhoto.Source = null;
        }

        private async void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthService authService = new(App.HttpClient);

            LoginDto login = new LoginDto()
            {
                Email = LoginTextBox.Text,
                Password = PasswordTextBox.Text,
            };

            try
            {
                AppState.AuthToken = await authService.GetTokenAsync(login);

                EmployeeService employeeService = new(App.HttpClient);

                Employees = await employeeService.GetAllProtectedAsync();

                EmployeesDataGrid.ItemsSource = Employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();
            dialog.Filter = "csv files|*.csv";

            if (dialog.ShowDialog() != true)
                return;

            var filename = dialog.FileName;

            if (File.Exists(filename))
                File.Delete(filename);

            foreach (var employee in Employees)
            {
                var line = String.Join(";", employee.EmployeeId, employee.Email, employee.Position.Name, Environment.NewLine);
                File.AppendAllText(filename, line, Encoding.UTF8);
            }
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "images| *.png;*.jpg|all files|*.*";

            if (dialog.ShowDialog() != true)
                return;

            var data = await File.ReadAllBytesAsync(dialog.FileName);

            if (data.Length > 10240)
            {
                MessageBox.Show("Слишком большой файл! Досвидания!");
                return;
            }

            var relativeFileName = Path.Combine("Photos", Path.GetFileName(dialog.FileName));

            File.Copy(dialog.FileName, Path.Combine(Environment.CurrentDirectory, relativeFileName));

            AppDbContext dbContext = new();
            Photo photo = new()
            {
                FileData = data,
                FileName = relativeFileName,
            };

            try
            {
                await dbContext.Photos.AddAsync(photo);
                await dbContext.SaveChangesAsync();
                MessageBox.Show("успех Досвидания !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
