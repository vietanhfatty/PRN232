using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly HospitalManagementDbContext _context;

    public MedicalRecordRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<MedicalRecord> GetMedicalRecords()
    {
        return MedicalRecordDAO.Instance.GetMedicalRecords(_context);
    }

    public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(int id)
    {
        return await MedicalRecordDAO.Instance.GetMedicalRecordByIdAsync(_context, id);
    }

    public async Task AddMedicalRecordAsync(MedicalRecord medicalRecord)
    {
        await MedicalRecordDAO.Instance.AddMedicalRecordAsync(_context, medicalRecord);
    }

    public async Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord)
    {
        await MedicalRecordDAO.Instance.UpdateMedicalRecordAsync(_context, medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await MedicalRecordDAO.Instance.DeleteMedicalRecordAsync(_context, id);
    }
}
