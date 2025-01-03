using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ServiceLayer.Data;
using ServiceLayer.DTOs;
using ServiceLayer.Services;
using WebApi.DTOs;

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


        /// <summary>
        /// POST: /api/v1/SignIn
        /// Вход в учетную запись, генерация JWT-токена
        /// </summary>
        /// <param name="employee">Объект содержащий email и пароль пользователя</param>
        /// <returns>JWT-токен</returns>
        /// <response code="200">Успешная авторизация</response>
        /// <response code="400">Неверные параметры</response>
        /// <response code="401">Не удалось авторизоваться</response>
        [HttpPost("SingIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorDto))]
        public IActionResult Login(LoginDto employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest(new ApiErrorDto("Почта не указана", 3000));
            if (string.IsNullOrWhiteSpace(employee.Password))
                return BadRequest(new ApiErrorDto("Пароль не указан", 3001));

            var dbEmployee = _context.Employees.FirstOrDefault(u => u.Email == employee.Email);
            if (dbEmployee is null)
                return NotFound(new ApiErrorDto("Пользователь не найден", 3002));
            if (dbEmployee.Password != employee.Password)
                return BadRequest(new ApiErrorDto("Почта или пароль неверны", 3003));
            return Ok(_tokenService.GenerateToken(dbEmployee));
        }
    }
}
