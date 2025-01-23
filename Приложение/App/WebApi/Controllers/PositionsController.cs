using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PositionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositionsAsync()
            => await _context.Positions.ToListAsync();
    }
}
