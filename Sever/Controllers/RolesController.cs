using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class RolesController : ODataController
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_roleRepository.GetRoles());
    }

    [HttpGet("/api/roles")]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleRepository.GetRoles().ToListAsync();
        return Ok(roles);
    }
}
