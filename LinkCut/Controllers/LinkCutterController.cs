using LinkCut.Data;
using LinkCut.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkCut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkCutterController(AppDbContext _context) : ControllerBase
    {
        
    }
}
