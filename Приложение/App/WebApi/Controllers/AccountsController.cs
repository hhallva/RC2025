using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicelayer.Data;
using Servicelayer.Models;
using Servicelayer.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AccountsController(TokenService tokenService, AppDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost]
        public IActionResult SignIn(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest("Почта не указана");
            if (string.IsNullOrWhiteSpace(employee.Password))
                return BadRequest("Пароль не указан");

            var dbUser = _context.Employees.FirstOrDefault(u => u.Email == employee.Email);
            if (dbUser is null)
                return NotFound("Пользователь не найден");
            if (dbUser.Password != employee.Password)
                return BadRequest("Почта или пароль неверны");
            string token = _tokenService.GenerateToken(employee);// для просмотра
            return Ok(_tokenService.GenerateToken(employee));
        }

    }
}
