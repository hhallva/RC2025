using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.Text;

namespace WebApp.Pages
{
    public class QrCodeModel(EmployeeService employeeService) : PageModel
    {
        public string? QrCodeImage { get; set; }

        public async Task OnGet(int employeeId)
        {
            var employee = (await employeeService.GetAllAsync()).SingleOrDefault(e => e.EmployeeId == employeeId);

            if (employee != null)
            {
                string data = GenerateData(employee);
                QrCodeImage = GenerateQrCode(data);
            }
        }

        private string GenerateQrCode(string data)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCode = new PngByteQRCode(qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q));
            return "data:image/png;base64," + Convert.ToBase64String(qrCode.GetGraphic(5));
        }

        private string GenerateData(Employee employee)
        {
            StringBuilder vCard = new();
            vCard.AppendLine("BEGIN:VCARD");
            vCard.AppendLine("VERSION:3.0");
            vCard.AppendLine($"N:{employee.Name}");
            vCard.AppendLine($"FN:{employee.Surname}");
            vCard.AppendLine($"ORG:ГК Дороги России");
            vCard.AppendLine($"TITLE:{employee.Position.Name}");
            vCard.AppendLine($"WORK:{employee.WorkPhone}");
            vCard.AppendLine($"TEL:{employee.Phone}");
            vCard.AppendLine($"EMAIL:{employee.Email}");
            vCard.AppendLine("END:VCARD");

            return vCard.ToString();
        }

    }
}
