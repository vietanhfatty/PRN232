using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class MedicineDAO
{
    private static MedicineDAO? _instance;
    private static readonly object _instanceLock = new object();

    private MedicineDAO() { }

    public static MedicineDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new MedicineDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Medicine> GetMedicines(HospitalManagementDbContext context)
    {
        return context.Medicines.AsQueryable();
    }

    public async Task<Medicine?> GetMedicineByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Medicines.FindAsync(id);
    }

    public async Task AddMedicineAsync(HospitalManagementDbContext context, Medicine medicine)
    {
        context.Medicines.Add(medicine);
        await context.SaveChangesAsync();
    }

    public async Task UpdateMedicineAsync(HospitalManagementDbContext context, Medicine medicine)
    {
        context.Entry(medicine).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteMedicineAsync(HospitalManagementDbContext context, int id)
    {
        var medicine = await context.Medicines.FindAsync(id);
        if (medicine != null)
        {
            context.Medicines.Remove(medicine);
            await context.SaveChangesAsync();
        }
    }
}
