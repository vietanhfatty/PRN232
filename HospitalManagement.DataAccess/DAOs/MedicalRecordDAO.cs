using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class MedicalRecordDAO
{
    private static MedicalRecordDAO? _instance;
    private static readonly object _instanceLock = new object();

    private MedicalRecordDAO() { }

    public static MedicalRecordDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new MedicalRecordDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<MedicalRecord> GetMedicalRecords(HospitalManagementDbContext context)
    {
        return context.MedicalRecords
            .Include(m => m.Appointment)
            .Include(m => m.Prescriptions)
                .ThenInclude(p => p.Medicine)
            .AsQueryable();
    }

    public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.MedicalRecords
            .Include(m => m.Appointment)
            .Include(m => m.Prescriptions)
                .ThenInclude(p => p.Medicine)
            .FirstOrDefaultAsync(m => m.RecordId == id);
    }

    public async Task AddMedicalRecordAsync(HospitalManagementDbContext context, MedicalRecord medicalRecord)
    {
        context.MedicalRecords.Add(medicalRecord);
        await context.SaveChangesAsync();
    }

    public async Task UpdateMedicalRecordAsync(HospitalManagementDbContext context, MedicalRecord medicalRecord)
    {
        context.Entry(medicalRecord).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteMedicalRecordAsync(HospitalManagementDbContext context, int id)
    {
        var record = await context.MedicalRecords.FindAsync(id);
        if (record != null)
        {
            context.MedicalRecords.Remove(record);
            await context.SaveChangesAsync();
        }
    }
}
