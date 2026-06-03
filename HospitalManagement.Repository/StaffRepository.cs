using HospitalManagement.DataAccess.DAOs;
using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository;

public class StaffRepository : IStaffRepository
{
    private readonly HospitalManagementDbContext _context;

    public StaffRepository(HospitalManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Staff> GetStaffs()
    {
        return StaffDAO.Instance.GetStaffs(_context);
    }

    public async Task<Staff?> GetStaffByIdAsync(int id)
    {
        return await StaffDAO.Instance.GetStaffByIdAsync(_context, id);
    }

    public async Task AddStaffAsync(Staff staff)
    {
        await StaffDAO.Instance.AddStaffAsync(_context, staff);
    }

    public async Task UpdateStaffAsync(Staff staff)
    {
        await StaffDAO.Instance.UpdateStaffAsync(_context, staff);
    }

    public async Task DeleteStaffAsync(int id)
    {
        await StaffDAO.Instance.DeleteStaffAsync(_context, id);
    }
}
