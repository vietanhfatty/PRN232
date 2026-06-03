using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IMedicalRecordRepository
{
    IQueryable<MedicalRecord> GetMedicalRecords();
    Task<MedicalRecord?> GetMedicalRecordByIdAsync(int id);
    Task AddMedicalRecordAsync(MedicalRecord medicalRecord);
    Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord);
    Task DeleteMedicalRecordAsync(int id);
}
