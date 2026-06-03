using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class MedicinesController : ODataController
{
    private readonly IMedicineRepository _medicineRepository;

    public MedicinesController(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_medicineRepository.GetMedicines());
    }

    [HttpGet("/api/medicines")]
    public async Task<IActionResult> GetAll()
    {
        var medicines = await _medicineRepository.GetMedicines().ToListAsync();
        return Ok(medicines);
    }
}
