using HospitalManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.DataAccess.DAOs;

public class StaffDAO
{
    private static StaffDAO? _instance;
    private static readonly object _instanceLock = new object();

    private StaffDAO() { }

    public static StaffDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new StaffDAO();
                }
                return _instance;
            }
        }
    }

    public IQueryable<Staff> GetStaffs(HospitalManagementDbContext context)
    {
        return context.Staffs.Include(s => s.Account).AsQueryable();
    }

    public async Task<Staff?> GetStaffByIdAsync(HospitalManagementDbContext context, int id)
    {
        return await context.Staffs
            .Include(s => s.Account)
            .FirstOrDefaultAsync(s => s.StaffId == id);
    }

    public async Task AddStaffAsync(HospitalManagementDbContext context, Staff staff)
    {
        context.Staffs.Add(staff);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStaffAsync(HospitalManagementDbContext context, Staff staff)
    {
        context.Entry(staff).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteStaffAsync(HospitalManagementDbContext context, int id)
    {
        var staff = await context.Staffs.FindAsync(id);
        if (staff != null)
        {
            context.Staffs.Remove(staff);
            await context.SaveChangesAsync();
        }
    }
}
