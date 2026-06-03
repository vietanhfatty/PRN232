using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class StaffsController : ODataController
{
    private readonly IStaffRepository _staffRepository;

    public StaffsController(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_staffRepository.GetStaffs());
    }

    [HttpGet("/api/staffs/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var staff = await _staffRepository.GetStaffByIdAsync(id);
        if (staff == null)
        {
            return NotFound();
        }
        return Ok(staff);
    }
}
