using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class PrescriptionDAO
{
    private static PrescriptionDAO? _instance;
    private static readonly object _instanceLock = new object();

    private PrescriptionDAO() { }

    public static PrescriptionDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new PrescriptionDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Prescription> GetPrescriptions(HospitalManagementDbContext context)
    {
        return context.Prescriptions
            .Include(p => p.Record)
            .Include(p => p.Medicine)
            .AsQueryable();
    }

    public async Task<Prescription?> GetPrescriptionByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Prescriptions
            .Include(p => p.Record)
            .Include(p => p.Medicine)
            .FirstOrDefaultAsync(p => p.PrescriptionId == id);
    }

    public async Task AddPrescriptionAsync(HospitalManagementDbContext context, Prescription prescription)
    {
        context.Prescriptions.Add(prescription);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePrescriptionAsync(HospitalManagementDbContext context, Prescription prescription)
    {
        context.Entry(prescription).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeletePrescriptionAsync(HospitalManagementDbContext context, int id)
    {
        var prescription = await context.Prescriptions.FindAsync(id);
        if (prescription != null)
        {
            context.Prescriptions.Remove(prescription);
            await context.SaveChangesAsync();
        }
    }
}
