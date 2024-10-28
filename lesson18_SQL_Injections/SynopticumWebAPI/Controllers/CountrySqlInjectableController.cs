using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Contract;
using SynopticumDAL.Services;
using SynopticumModel.Entities;

namespace SynopticumWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountrySqlInjectableController(
    IUnitOfWork _unitOfWork,
    SynopticumDbContext _dbContext)
    : ControllerBase
{
    /// <summary>
    /// the usual modern LINQ-based - secure by default
    /// </summary>
    [HttpGet("Secure")]
    public async Task<ActionResult> Get(string countryNamePart)
    {
        var repo = _unitOfWork.GetRepository<Country>();

        var users = await repo.AsReadOnlyQueryable()
            .Where(u => u.Name.Contains(countryNamePart))
            .ToListAsync();

        return new JsonResult(users);
    }


    /// <summary>
    /// the usual modern LINQ-based - secure by default
    /// </summary>
    [HttpGet("Insecure")]
    public async Task<ActionResult> GetInsecure(string countryNamePart)
    {
        var dbset = _dbContext.Set<Country>();

        // more secure way - use Formattable String and SqlParameter (?!)
        //// todo somehow the parameter doesn't get inserted at all
        //FormattableString req = $"select a.* from \"AspNetUsers\" as a where a.\"Email\" like '%{searchByEmail}%'";
        //var users = await dbset.FromSql<User>(req).ToListAsync();

        // totally not safe!
        var req_raw = $"select * from Country where Name like '%{countryNamePart}%'";
        var users = await dbset.FromSqlRaw<Country>(req_raw).ToListAsync();

        return new JsonResult(users);
    }
}
