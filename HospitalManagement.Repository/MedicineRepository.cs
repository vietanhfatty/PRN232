using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class MedicineRepository : IMedicineRepository
{
    private readonly HospitalManagementDbContext _context;

    public MedicineRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Medicine> GetMedicines()
    {
        return MedicineDAO.Instance.GetMedicines(_context);
    }

    public async Task<Medicine?> GetMedicineByIdAsync(int id)
    {
        return await MedicineDAO.Instance.GetMedicineByIdAsync(_context, id);
    }

    public async Task AddMedicineAsync(Medicine medicine)
    {
        await MedicineDAO.Instance.AddMedicineAsync(_context, medicine);
    }

    public async Task UpdateMedicineAsync(Medicine medicine)
    {
        await MedicineDAO.Instance.UpdateMedicineAsync(_context, medicine);
    }

    public async Task DeleteMedicineAsync(int id)
    {
        await MedicineDAO.Instance.DeleteMedicineAsync(_context, id);
    }
}
