using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Data;
using ServiceLayer.Dtos;
using ServiceLayer.Services;

namespace WebApi.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AccountController(TokenService tokenService, AppDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("SingIn")]
        public IActionResult Login(EmployeeDto employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest("Почта не указана");
            if (string.IsNullOrWhiteSpace(employee.Password))
                return BadRequest("Пароль не указан");

            var dbEmployee = _context.Employees.FirstOrDefault(u => u.Email == employee.Email);
            if (dbEmployee is null)
                return NotFound("Пользователь не найден");
            if (dbEmployee.Password != employee.Password)
                return BadRequest("Почта или пароль неверны");
            return Ok(_tokenService.GenerateToken(dbEmployee));
        }
    }
}
