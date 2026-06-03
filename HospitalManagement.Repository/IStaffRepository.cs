using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public interface IStaffRepository
{
    IQueryable<Staff> GetStaffs();
    Task<Staff?> GetStaffByIdAsync(int id);
    Task AddStaffAsync(Staff staff);
    Task UpdateStaffAsync(Staff staff);
    Task DeleteStaffAsync(int id);
}
