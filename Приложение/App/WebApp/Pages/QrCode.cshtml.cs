using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.Text;

namespace WebApp.Pages
{
    public class QrCodeModel(EmployeeService employeeService) : PageModel
    {
        private Employee employee;

        public string? QrCodeImage { get; set; }

        public async Task OnGetAsync(int employeeId)
        {
            employee = await employeeService.GetAsync(employeeId);

            if (employee != null)
            {
                string data = GenerateVCard();
                QrCodeImage = GenerateQrCode(data);
            }
        }

        private string GenerateQrCode(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCode = new PngByteQRCode(qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q));
            return "data:image/png;base64," + Convert.ToBase64String(qrCode.GetGraphic(5));
        }

        private string GenerateVCard()
        {
            StringBuilder dataVCard = new();
            dataVCard.AppendLine("BEGIN:VCARD");
            dataVCard.AppendLine("VERSION:3.0");
            dataVCard.AppendLine($"N:{employee.Name}");
            dataVCard.AppendLine($"FM:{employee.Surname}");
            dataVCard.AppendLine($"ORG:ГК Дороги России");
            dataVCard.AppendLine($"TITLE:{employee.Position.Name}");
            dataVCard.AppendLine($"WORK:{employee.WorkPhone}");
            dataVCard.AppendLine($"EMAIL:{employee.Email}");
            dataVCard.AppendLine("END:VCARD");

            return dataVCard.ToString();
        }
    }
}
