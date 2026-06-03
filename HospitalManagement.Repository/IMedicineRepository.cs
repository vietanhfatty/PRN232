using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IMedicineRepository
{
    IQueryable<Medicine> GetMedicines();
    Task<Medicine?> GetMedicineByIdAsync(int id);
    Task AddMedicineAsync(Medicine medicine);
    Task UpdateMedicineAsync(Medicine medicine);
    Task DeleteMedicineAsync(int id);
}
